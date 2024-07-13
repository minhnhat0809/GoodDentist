using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Repositories;
using static StackExchange.Redis.Role;

namespace Services.Impl
{
	public class PaymentService : IPaymentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public PaymentService (IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<ResponseDTO> GetAllPayment(int pageNumber, int rowsPerPage)
		{
			try
			{
				List<Payment> list = await _unitOfWork.paymentRepo.GetAllPayment(pageNumber, rowsPerPage);
				return new ResponseDTO("Get payments successfully!", 200, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO("Failed to get payments", 500, true, null);
			}

		}
	}
}
