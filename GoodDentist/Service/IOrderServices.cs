using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;

namespace Services
{
	public interface IOrderServices
	{
		Task<ResponseDTO> GetAllOrder(int pageNumber, int pageSize);

		Task<ResponseDTO> SearchOrder(string searchValue);

		Task<ResponseDTO> AddOrder(OrderDTO orderDTO);

		Task<ResponseDTO> UpdateOrder(OrderDTO orderDTO);

		Task<ResponseDTO> DeleteOrder(int orderId);
	}
}
