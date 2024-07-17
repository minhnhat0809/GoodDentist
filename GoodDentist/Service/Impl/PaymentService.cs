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
                        Payment payment = new Payment()
                        {
                            Price = order.Price,
                            Status = true,
                            Order = order,
                            PaymentDetail = order.OrderName + " : " + order.Price,
                            OrderId = order.OrderId,
                            CreateAt = DateTime.Now
                        };
                        model.PaymentOrder = payment;
                        model.Total += payment.Price;   
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
                        // check order - [payment for all order's service in one time]
                        if (paymentDTO.Order != null)
                        {
                            //return new ResponseDTO("We are not support this feature yet!", 400, false, null);
                            Order? order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId);
                            if (order != null && order.Status == true)
                            {
                                Payment paymentOrder = new Payment()
                                {
                                    Price = order.Price,
                                    Status = true,
                                    CreateAt = DateTime.Now,
                                    Order = order,
                                    OrderId = order.OrderId
                                };
                                model.PaymentOrder = paymentOrder;
                                model.Total += paymentOrder.Price;
                            } else return new ResponseDTO("Order had Paid!", 400, false, _mapper.Map<OrderDTO>(order));
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
                                   // model.Total += prescription.Total;
                                    
                                } else return new ResponseDTO("Prescription had Paid!", 400, false, _mapper.Map<PrescriptionDTO>(prescription));
                            }

                            if (paymentDTO.Order != null)
                            {
                                Order? order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId);
                                if (order != null && order.Status == true)
                                {
                                    var checkUpdateOrder = await _orderServices.UpdateOrderAfterPayment(order);
                                    if (!checkUpdateOrder.IsSuccess) return checkUpdateOrder;
                                    // if success add price to payment-all
                                    //model.Total += order.Price;
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

        public async Task<ResponseDTO> GetPaymentsPerYear(int year)
        {
            ResponseDTO responseDto = new ResponseDTO("",200,true,null);
            try
            {
                List<PaymentAll> paymentAlls = await _unitOfWork.paymentAllRepo.GetPaymentsPerYear(year);

                if (paymentAlls.IsNullOrEmpty())
                {
                    responseDto.Message = "This year has no income!";
                    return responseDto;
                }

                
                var months = new Dictionary<int, PaymentPerYearDTO>
                {
                    { 1, new PaymentPerYearDTO("January", 0) },
                    { 2, new PaymentPerYearDTO("February", 0) },
                    { 3, new PaymentPerYearDTO("March", 0) },
                    { 4, new PaymentPerYearDTO("April", 0) },
                    { 5, new PaymentPerYearDTO("May", 0) },
                    { 6, new PaymentPerYearDTO("June", 0) },
                    { 7, new PaymentPerYearDTO("July", 0) },
                    { 8, new PaymentPerYearDTO("August", 0) },
                    { 9, new PaymentPerYearDTO("September", 0) },
                    { 10, new PaymentPerYearDTO("October", 0) },
                    { 11, new PaymentPerYearDTO("November", 0) },
                    { 12, new PaymentPerYearDTO("December", 0) }
                };

                
                foreach (var pa in paymentAlls)
                {
                    if (pa.Date.HasValue && pa.Total.HasValue)
                    {
                        months[pa.Date.Value.Month].Income += pa.Total.Value;
                    }
                }
                
                responseDto.Result = months.Values.ToList();
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.StatusCode = 500;
                responseDto.Message = e.Message;
            }
            return responseDto;
        }

        public async Task<ResponseDTO> GetPaymentsInDateRange(DateOnly DateStart, DateOnly DateEnd)
        {
            ResponseDTO responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                List<PaymentAll> paymentAlls = await _unitOfWork.paymentAllRepo.GetPaymentsInRange(DateStart, DateEnd);
                if (paymentAlls.IsNullOrEmpty())
                {
                    responseDto.Message = "There is no income in this range";
                    responseDto.Result = 0;
                    return responseDto;
                }

                decimal total = 0;

                foreach (var pa in paymentAlls)
                {
                    if (pa.Date.HasValue && pa.Total.HasValue)
                    {
                        total += pa.Total.Value;
                    }
                }

                responseDto.Result = total;
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.StatusCode = 500;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }

        public async Task<ResponseDTO> GetPaymentsOfServicesInDateRange(DateOnly DateStart, DateOnly DateEnd)
        {
            ResponseDTO responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                List<OrderService> orderServices = await _unitOfWork.serviceRepo.GetServiceUsedInDateRange(DateStart, DateEnd);

                List<Service> services = await _unitOfWork.serviceRepo.GetAllService(1, 300);

                var serviceDtos = new Dictionary<int, PaymentServiceDTO>();
                foreach (var s in services)
                {
                   serviceDtos.Add(s.ServiceId, new PaymentServiceDTO(s.ServiceName, 0)); 
                }
                
                
                foreach (var os in orderServices)
                {
                    if (os.Price.HasValue && os.ServiceId.HasValue)
                    {
                        serviceDtos[os.ServiceId.Value].Total += os.Price.Value;
                    }
                }

                responseDto.Result = serviceDtos.Values.ToList();
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.StatusCode = 500;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }
    }
}
