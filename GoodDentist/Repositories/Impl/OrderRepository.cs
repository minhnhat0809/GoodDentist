using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;

namespace Repositories.Impl
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
        public OrderRepository(GoodDentistDbContext goodDentistDbContext) : base(goodDentistDbContext) 
        { 
        }

		public async Task<List<Order>?> GetAllOrder(int pageNumber, int pageSize)
		{
			List<Order> orders = await Paging(pageNumber, pageSize);
			return orders;
		}
	}
}
