using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;
using Microsoft.Extensions.Caching.Distributed;

namespace Repositories.Impl
{
	public class PaymentRepo : RepositoryBase<Payment>, IPaymentRepo
	{
		private readonly IDistributedCache _distributedCache;
		public PaymentRepo(GoodDentistDbContext repositoryContext, IDistributedCache distributedCache) : base(repositoryContext)
		{
			_distributedCache = distributedCache;
		}

		public async Task<List<Payment>> GetAllPayment(int pageNumber, int rowsPerPage)
		{
			List<Payment> list = await Paging(pageNumber, rowsPerPage);
			list.Where(s => true).ToList();
			return list;
		}
	}
}
