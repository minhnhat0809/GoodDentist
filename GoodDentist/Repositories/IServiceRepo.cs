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

		Task<List<Service>> GetServicesByClinic(string clinicId, int pageNumber, int rowsPerPage);
		
		Task<List<Service>> GetServicesByClinicByFilter(string filterValue, string clinicId, int pageNumber, int rowsPerPage);
		Task<List<Service>> GetServicesByClinicByFilterNoPaging(string filterValue, string clinicId);
		Task<List<Service>> GetAllServiceNoPaging();

		Task<List<OrderService>> GetServiceUsedInDateRange(DateTime DateStart, DateTime DateEnd);
	}
}
