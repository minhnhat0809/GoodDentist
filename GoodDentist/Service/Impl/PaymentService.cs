using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.PaymentDTOs;
using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMedicineService _medicineService;
        private readonly IOrderServices _orderServices;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IMedicineService medicineService, IOrderServices orderServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _medicineService = medicineService;
            _orderServices = orderServices;
        }

        public async Task<ResponseDTO> GetAllPayment(int pageNumber, int rowsPerPage)
        {
            try
            {
                List<PaymentAll> list = await _unitOfWork.paymentAllRepo.GetAllPayment(pageNumber, rowsPerPage);
                return new ResponseDTO("Get payments successfully!", 200, true, _mapper.Map<List<PaymentAllDTO>>(list));
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to get payments", 500, false, ex.Message);
            }
        }

        public async Task<ResponseDTO> GetPaymentById(int id)
        {
            try
            {
                PaymentAll payment = await _unitOfWork.paymentAllRepo.GetPaymentById(id);
                if (payment == null)
                {
                    return new ResponseDTO("Payment not found", 404, false, null);
                }
                var paymentDTO = _mapper.Map<PaymentAllDTO>(payment);
                return new ResponseDTO("Get payment successfully!", 200, true, paymentDTO);
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to get payment", 500, false, ex.Message);
            }
        }

        public async Task<ResponseDTO> CreatePayment(PaymentAllCreateDTO paymentDTO)
        {
            try
            {
                var model = _mapper.Map<PaymentAll>(paymentDTO);
                model.Total = 0;
                model.Status = true;
                // check prescription
                if (paymentDTO.Prescription != null)
                {
                    Prescription? prescription =
                        await _unitOfWork.prescriptionRepo.GetPrescriptionById(paymentDTO.Prescription.PrescriptionId);
                    if (prescription != null && prescription.Status == true)
                    {
                        PaymentPrescription paymentPrescription = new PaymentPrescription()
                        {
                            Prescription = prescription,
                            Status = true,
                            PrescriptionId = prescription.PrescriptionId,
                            Price = prescription.Total,
                            PaymentDetail = prescription.Note
                        };
                        model.PaymentPrescription = paymentPrescription;
                        model.Total += paymentPrescription.Price;
                    }
                }
                // check order
                if (paymentDTO.Order != null)
                {
                    Order? order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId);
                    if (order != null && order.Status == true)
                    {
                        foreach (OrderService orderService in order.OrderServices)
                        {
                            Payment payment = new Payment()
                            {
                                Price = orderService.Price * orderService.Quantity, // price * quantity
                                Status = true,
                                OrderService = orderService,
                                PaymentDetail = orderService.Quantity.ToString(),
                                OrderServiceId = orderService.OrderId,
                                CreateAt = DateTime.Now
                            };
                            model.Payments.Add(payment);
                            model.Total += payment.Price;
                        }
                    }
                }
                await _unitOfWork.paymentAllRepo.CreatePayment(model);
                return new ResponseDTO("Create payment successfully!", 201, true, _mapper.Map<PaymentAllDTO>(model));
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to create payment", 500, false, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdatePayment(PaymentAllUpdateDTO paymentDTO)
        {
            try
            {
                PaymentAll model = await _unitOfWork.paymentAllRepo.GetPaymentById(paymentDTO.PaymentAllId);
                if (model != null)
                {
                    if (model.Status == true)
                    {
                        model = _mapper.Map<PaymentAll>(paymentDTO);
                        //model.PaymentDetail = paymentDTO.PaymentDetail;
                        
                        model.Total = 0;
                        // check prescription - done
                        if (paymentDTO.Prescription != null)
                        {
                            Prescription? prescription =
                                await _unitOfWork.prescriptionRepo.GetPrescriptionById(paymentDTO.Prescription
                                    .PrescriptionId);
                            if (prescription != null && prescription.Status == true)
                            {
                                PaymentPrescription paymentPrescription = new PaymentPrescription()
                                {
                                    Prescription = prescription,
                                    Status = true,
                                    PrescriptionId = prescription.PrescriptionId,
                                    Price = prescription.Total,
                                    PaymentDetail = prescription.Note
                                };
                                model.PaymentPrescription = paymentPrescription;
                                model.Total += paymentPrescription.Price;
                            }else return new ResponseDTO("Prescription had Paid!", 400, false, _mapper.Map<PrescriptionDTO>(prescription));
                        }
                        // check order - [payment for all order in one time]
                        if (paymentDTO.Order != null)
                        {
                            return new ResponseDTO("We are not support this feature yet!", 400, false, null);
                            Order? order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId);
                            if (order != null && order.Status == true)
                            {
                                foreach (OrderService orderService in order.OrderServices)
                                {
                                    List<Payment> payList =
                                        (await _unitOfWork.paymentAllRepo.GetPaymentById(paymentDTO.PaymentAllId))
                                        .Payments.ToList();
                                    if (!payList.IsNullOrEmpty())
                                    {
                                        Payment? paymentOrder = payList.FirstOrDefault(x =>
                                            x.OrderServiceId == orderService.OrderServiceId && x.Status == true);
                                        // check if Exist( orderCreate) == Order get by id =>> create Payment [when status= Paid set OrderService = false (Paid)]
                                        if(paymentOrder!= null)
                                        {
                                            Payment payment = new Payment()
                                            {
                                                Price = orderService.Price * orderService.Quantity, // price * quantity
                                                Status = true,
                                                OrderService = orderService,
                                                PaymentDetail = orderService.Quantity.ToString(),
                                                OrderServiceId = orderService.OrderId,
                                                CreateAt = DateTime.Now
                                            };
                                            model.Payments.Add(payment);
                                        }
                                    }
                                    
                                }
                            }else return new ResponseDTO("Order had Paid!", 400, false, _mapper.Map<OrderDTO>(order));
                        }
                        // check is Paid yet ?
                        if (paymentDTO.Status == true)
                        {
                           await _unitOfWork.paymentAllRepo.UpdatePayment(model);
                            return new ResponseDTO("Update payment successfully!", 200, true, paymentDTO);
                        }
                        else if (paymentDTO.Status == false)
                        {
                            //done
                            if (paymentDTO.Prescription != null)
                            {
                                Prescription? prescription =
                                    await _unitOfWork.prescriptionRepo.GetPrescriptionById(paymentDTO.Prescription
                                        .PrescriptionId);
                                if (prescription!= null && prescription.Status == true)
                                {
                                    // Update Medicine Storage
                                    // UpdateMedicineAfterPayment(int PrescriptionId)
                                    var checkUpdateMedicine =
                                        await _medicineService.UpdateMedicineAfterPaymentPrescription(paymentDTO
                                            .Prescription
                                            .PrescriptionId);
                                    if (!checkUpdateMedicine.IsSuccess)
                                        return checkUpdateMedicine;
                                    // if success add price to payment-all
                                    model.Total += prescription.Total;
                                    
                                } else return new ResponseDTO("Prescription had Paid!", 400, false, _mapper.Map<PrescriptionDTO>(prescription));
                            }

                            if (paymentDTO.Order != null)
                            {
                                Order? order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId);
                                if (order != null && order.Status == true )
                                {
                                    List<Payment> payList =
                                        (await _unitOfWork.paymentAllRepo.GetPaymentById(paymentDTO.PaymentAllId))
                                        .Payments.ToList();
                                    
                                    foreach (OrderService orderService in order.OrderServices)
                                    {
                                        var tmp = model.Payments.FirstOrDefault(x =>
                                            x.OrderServiceId == orderService.OrderServiceId && x.Status == true);
                                        if (tmp != null)
                                        {
                                            model.Payments.Remove(tmp); // Remove found service of order

                                            Payment? paymentOrder = payList.FirstOrDefault(x =>
                                                x.OrderServiceId == orderService.OrderServiceId && x.Status == true);
                                            // check if Any( orderCreate) == Order get by id 
                                            if (paymentOrder != null)
                                            {
                                                paymentOrder.Status = false;
                                                model.Payments.Add(paymentOrder); // set again service had Paid 

                                            }
                                        }
                                    }
                                    // Update Order [include List<OrderService> where status false] 
                                    var checkOrder =
                                        await _orderServices.UpdateOrderAfterPayment(
                                            paymentDTO.Order.OrderId
                                            , model.Payments.ToList());
                                    if (checkOrder.IsSuccess) return checkOrder;
    
                                } else return new ResponseDTO("Order had Paid!", 400, false, _mapper.Map<OrderDTO>(order));
                            }
                            // UPDATE - PAYMENT ALL
                            await _unitOfWork.paymentAllRepo.UpdatePayment(model);
                            return new ResponseDTO("Paying successfully!", 200, true, paymentDTO);
                        }
                    } return new ResponseDTO("This payment being paid!", 400, false, _mapper.Map<PaymentAllDTO>(model));
                }
                return new ResponseDTO("Payment not found!", 400, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to update payment", 500, false, ex.Message);
            }
        }

        public async Task<ResponseDTO> DeletePayment(int id)
        {
            try
            {
                PaymentAll model = await _unitOfWork.paymentAllRepo.GetPaymentById(id);
                if (model != null)
                {
                    if (model.Status == true)
                    {
                        await _unitOfWork.paymentAllRepo.DeletePayment(id);
                        return new ResponseDTO("Delete payment successfully!", 200, true, _mapper.Map<PaymentAllDTO>(model));
                    }
                    return new ResponseDTO("This payment being paid!", 999, false, _mapper.Map<PaymentAllDTO>(model));
                }
                return new ResponseDTO("Payment not found!", 400, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to delete payment", 500, false, ex.Message);
            }
        }
    }
}
