using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;

namespace Repositories
{
	public interface IServiceRepo : IRepositoryBase<Service>
	{
		Task<List<Service>> GetAllService(int pageNumber, int rowsPerPage);
		Task<Service> GetServiceByID(int serviceId);
	}
}
