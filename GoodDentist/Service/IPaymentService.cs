using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.PaymentDTOs.View;

namespace Services
{
	public interface IPaymentService
	{
		Task<ResponseDTO> GetPaymentById(int id);
		Task<ResponseDTO> GetAllPayment(int pageNumber, int rowsPerPage);
		Task<ResponseDTO> CreatePayment(PaymentAllDTO paymentDTO);
		Task<ResponseDTO> UpdatePayment(PaymentAllDTO paymentDTO);
		Task<ResponseDTO> DeletePayment(int id);
	}
}
