using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.Extensions.Caching.Distributed;

namespace Repositories.Impl
{
	public class ServiceRepo : RepositoryBase<Service>, IServiceRepo
	{
		private readonly IDistributedCache distributedCache;
		public ServiceRepo(GoodDentistDbContext repositoryContext, IDistributedCache distributedCache) : base(repositoryContext)
		{
			this.distributedCache = distributedCache;
		}


		public async Task<List<Service>> GetAllService(int pageNumber, int rowsPerPage)
		{
			List<Service> serviceList = await Paging(pageNumber, rowsPerPage);
			serviceList.Where(s => true).ToList();
			return serviceList;
		}

		public async Task<Service> GetServiceByID(int serviceId)
		{
			Service service = _repositoryContext.Services.Where(t=>t.ServiceId==serviceId).FirstOrDefault();
			return service;
		}
	}
}

