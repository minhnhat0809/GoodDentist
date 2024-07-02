using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
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
			try
			{
				List<Order>? orderList = await _unitOfWork.orderRepo.GetAllOrder(pageNumber, pageSize);
				var all = orderList.Where(c => c.Status == true);

				List<OrderDTO> orderDTOList = _mapper.Map<List<OrderDTO>>(all);
				return new ResponseDTO("Get all Order successfully!", 200, true,orderDTOList);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> SearchOrder(string searchValue)
		{
			try
			{
				List<Order>? orderList = await _unitOfWork.orderRepo.FindByConditionAsync(c => c.OrderName == searchValue);
				var all = orderList.Where(c => c.Status == true);
				List<OrderDTO> orderDTOList = _mapper.Map<List<OrderDTO>>(all);
				if (orderDTOList.IsNullOrEmpty())
				{
					return new ResponseDTO("No result found!", 200, true, null);
				}

				return new ResponseDTO("Search Order successfully!", 200, true, orderDTOList);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> DeleteOrder(int orderId)
		{
			try
			{
				var order = await _unitOfWork.orderRepo.GetByIdAsync(orderId);
				if (order == null)
				{
					return new ResponseDTO("This order is not exist!", 400, false, null);
				}
				order.Status = false;
				var result = await _unitOfWork.orderRepo.DeleteAsync(order);
				if (result)
				{
					return new ResponseDTO("Order Delete succesfully!", 201, true, null);
				}
				return new ResponseDTO("Order Delete unsucessfully!", 400, false, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> AddOrder(OrderCreateDTO orderDTO)
		{
			try
			{
				var check = await CheckValidationAddOrder(orderDTO);
				if (check.IsSuccess == false)
				{
					return check;
				}

				Order order = _mapper.Map<Order>(orderDTO);
				await _unitOfWork.orderRepo.CreateAsync(order);
				return new ResponseDTO("Creat succesfully", 200, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> UpdateOrder(OrderDTO orderDTO)
		{
			try
			{
				var order = await _unitOfWork.orderRepo.GetByIdAsync(orderDTO.OrderId);
				if (order == null)
				{
					return new ResponseDTO("This order is not exist!", 400, false, null);
				}
				var check = await CheckValidationUpdateOrder(orderDTO);
				if (check.IsSuccess == false)
				{
					return check;
				}
				int orderId = order.OrderId;

				_unitOfWork.orderRepo.Detach(order);

				var updateOrder = _mapper.Map<Order>(orderDTO);

				updateOrder.OrderId = orderId;

				_unitOfWork.orderRepo.Attach(updateOrder);

				await _unitOfWork.orderRepo.UpdateAsync(updateOrder);
				return new ResponseDTO("Update Sucessfully!", 201, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> CheckValidationAddOrder(OrderCreateDTO orderDTO)
		{
			if (orderDTO.OrderName.IsNullOrEmpty())
			{
				return new ResponseDTO("Please input order name", 400, false, null);
			}

			/*if(orderDTO.ExaminationId.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input examinationID", 400, false, null);
			}*/

			if (orderDTO.DateTime.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please choose Date Time!", 400, false, null);
			}

			if(orderDTO.Price.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input order's price!", 400, false, null);
			}

			List<Order> order = await _unitOfWork.orderRepo.FindByConditionAsync(c => c.Status == true);
			if (order.Any(c => c.OrderName == orderDTO.OrderName))
			{
				return new ResponseDTO("Order name is already existed!", 400, false, null);
			}

			if(orderDTO.Price < 0)
			{
				return new ResponseDTO("Order's price must be greater than 0!", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}

		public async Task<ResponseDTO> CheckValidationUpdateOrder(OrderDTO orderDTO)
		{
			if(orderDTO.OrderId.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please choose order!", 400, false, null);
			}
			if (orderDTO.OrderName.IsNullOrEmpty())
			{
				return new ResponseDTO("Please input order name", 400, false, null);
			}

			/*if (orderDTO.ExaminationId.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input examinationID", 400, false, null);
			}*/

			if (orderDTO.DateTime.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please choose Date Time!", 400, false, null);
			}

			if (orderDTO.Price.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input order's price!", 400, false, null);
			}

			List<Order> orders = await _unitOfWork.orderRepo.FindByConditionAsync(c => c.Status == true);
			if (orders.Any(c => c.OrderName == orderDTO.OrderName && c.OrderId != orderDTO.OrderId))
			{
				return new ResponseDTO("Order name is already existed!", 400, false, null);
			}

			if (orderDTO.Price < 0)
			{
				return new ResponseDTO("Order's price must be greater than 0!", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}

	}
}
