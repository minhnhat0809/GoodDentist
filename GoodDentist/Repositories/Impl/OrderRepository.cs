using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
	{
        public OrderRepository(GoodDentistDbContext goodDentistDbContext) : base(goodDentistDbContext) 
        { 
        }

		public async Task<List<Order>?> GetAllOrder(int pageNumber, int pageSize)
		{
			List<Order> orders = await _repositoryContext.Orders
				.Include(x=>x.Examination)
				.Include(x=>x.OrderServices)
				.ThenInclude(x=>x.Service)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return orders;
		}

		public async Task<Order?> GetOrderById(int orderId)
		{
			return await _repositoryContext.Orders
				.Include(o => o.OrderServices).ThenInclude(os => os.Service)
				.Include(x=>x.Examination)
				.FirstOrDefaultAsync(o => o.OrderId == orderId);
		}

		public async Task<Order> DeleteOrder(int orderId)
		{
			var model = await _repositoryContext.Orders
				.Include(x => x.OrderServices)
				.ThenInclude(x => x.Service)
				.FirstOrDefaultAsync(x => x.OrderId == orderId);

			if (model != null)
			{
				model.Status = false;
					
				foreach (var os in model.OrderServices)
				{
					os.Status = 0;
				}
				await _repositoryContext.SaveChangesAsync();
					
				return model;
			}

			return null;
		}

		public async Task<Order> CreateOrder(Order order)
		{
			var model = await _repositoryContext.Orders.FirstOrDefaultAsync(x => x.OrderName == order.OrderName);
			if (model== null)
			{
				_repositoryContext.Orders.Add(order);
				_repositoryContext.SaveChanges();
				return  await _repositoryContext.Orders.FirstOrDefaultAsync(x => x.OrderName == order.OrderName);
			}

			return null;
		}

		public async Task<Order> UpdateOrder(Order order)
		{
				var model = await _repositoryContext.Orders
					.Include(x => x.OrderServices)
					.ThenInclude(x => x.Service)
					.FirstOrDefaultAsync(x => x.OrderName == order.OrderName);

				if (model != null)
				{
					_repositoryContext.Entry(model).CurrentValues.SetValues(order);
					_repositoryContext.OrderServices.RemoveRange(model.OrderServices);
					//await _repositoryContext.SaveChangesAsync();
					
					foreach (var os in order.OrderServices.ToList())
					{
						model.OrderServices.Add(os);	
					}
					//await _repositoryContext.SaveChangesAsync();
					
					return model;
				}

				return null;
		
		}
	}
}
