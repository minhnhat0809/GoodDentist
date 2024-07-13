using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entity;

namespace Repositories
{
	public interface IOrderRepository : IRepositoryBase<Order>
	{
		Task<List<Order>?> GetAllOrder(int pageNumber, int pageSize);

		Task<Order?> GetOrderById(int orderId);

		Task<Order> DeleteOrder(int orderId);
		Task<Order> CreateOrder(Order order);
		Task<Order> UpdateOrder(Order order);
	}
}
