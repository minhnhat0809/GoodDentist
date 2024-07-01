using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Repositories;
using Repositories.Impl;
using static StackExchange.Redis.Role;

namespace Services.Impl
{
	public class OrderServices : IOrderServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        public OrderServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
        }
		public async Task<ResponseDTO> GetAllOrder(int pageNumber, int pageSize)
		{
			ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
			try
			{
				List<Order>? orderList = await _unitOfWork.orderRepo.GetAllOrder(pageNumber, pageSize);
				var all = orderList.Where(c => c.Status == true);

				List<OrderDTO> orderDTOList = _mapper.Map<List<OrderDTO>>(all);
				responseDTO.Message = "Get all Order successfully!";
				responseDTO.Result = orderDTOList;
				return responseDTO;
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> SearchOrder(string searchValue)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> DeleteOrder(int orderId)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> AddOrder(OrderDTO orderDTO)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> UpdateOrder(OrderDTO orderDTO)
		{
			throw new NotImplementedException();
		}
	}
}
