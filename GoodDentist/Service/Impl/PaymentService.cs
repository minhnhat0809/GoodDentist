using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.PaymentDTOs;
using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> GetAllPayment(int pageNumber, int rowsPerPage)
        {
            try
            {
                List<PaymentAll> list = await _unitOfWork.paymentAllRepo.GetAllPayment(pageNumber, rowsPerPage);
                var paymentDTOs = _mapper.Map<List<PaymentAllDTO>>(list);
                return new ResponseDTO("Get payments successfully!", 200, true, paymentDTOs);
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
                if (paymentDTO.Prescription != null)
                {
                    Prescription prescription =
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
                    }
                }
                
                // if (paymentDTO.Order != null)
                // {
                //     Order order = await _unitOfWork.orderRepo.GetOrderById(paymentDTO.Order.OrderId.Value);
                //     if (order != null && order.Status == false)
                //     {
                //         foreach (OrderService orderService in order.OrderServices)
                //         {
                //             Payment payment = new Payment()
                //             {
                //                 Price = orderService.Price,
                //                 Status = true,
                //                 OrderService = orderService,
                //                 PaymentDetail = orderService.Quantity.ToString(),
                //                 OrderServiceId = orderService.OrderId
                //             };
                //             model.Payment = payment;
                //         }
                //     }
                // }
                
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
                            }
                        }

                        if (paymentDTO.Status == true)
                        {
                           
                            await _unitOfWork.paymentAllRepo.UpdatePayment(model);
                            return new ResponseDTO("Update payment successfully!", 200, true, paymentDTO);
                        }
                        else if (paymentDTO.Status == false)
                        {
                            // Update Medicine Storage
                            // UpdateMedicineAfterPayment(int PrescriptionId)
                            await _unitOfWork.paymentAllRepo.UpdatePayment(model);
                            return new ResponseDTO("Paying successfully!", 200, true, paymentDTO);
                        }
                    } return new ResponseDTO("This payment being paid!", 999, false, _mapper.Map<PaymentAllDTO>(model));
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
