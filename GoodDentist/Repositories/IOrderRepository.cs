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
	}
}
