using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.Entity;
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

        public async Task<ResponseDTO> CreatePayment(PaymentAllDTO paymentDTO)
        {
            try
            {
                var payment = _mapper.Map<PaymentAll>(paymentDTO);
                await _unitOfWork.paymentAllRepo.CreatePayment(payment);
                return new ResponseDTO("Create payment successfully!", 201, true, paymentDTO);
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to create payment", 500, false, ex.Message);
            }
        }

        public async Task<ResponseDTO> UpdatePayment(PaymentAllDTO paymentDTO)
        {
            try
            {
                var payment = _mapper.Map<PaymentAll>(paymentDTO);
                await _unitOfWork.paymentAllRepo.UpdatePayment(payment);
                return new ResponseDTO("Update payment successfully!", 200, true, paymentDTO);
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
                await _unitOfWork.paymentAllRepo.DeletePayment(id);
                return new ResponseDTO("Delete payment successfully!", 200, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO("Failed to delete payment", 500, false, ex.Message);
            }
        }
    }
}
