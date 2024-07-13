using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.OrderDTOs;
using BusinessObject.DTO.OrderDTOs.View;

namespace Services
{
    public interface IOrderServices
	{
		Task<ResponseDTO> GetAllOrder(int pageNumber, int pageSize);

		Task<ResponseDTO> SearchOrder(string searchValue);

		Task<ResponseDTO> AddOrder(OrderCreateDTO orderDTO);

		Task<ResponseDTO> UpdateOrder(OrderUpdateDTO orderDTO);

		Task<ResponseDTO> DeleteOrder(int orderId);

        Task<ResponseDTO> GetOrderDetails(int orderId);
    }
}
