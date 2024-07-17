using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Repositories.Impl;

public class ServiceRepo : RepositoryBase<Service>, IServiceRepo
{
	public ServiceRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
	{
	}


	public async Task<List<Service>> GetAllService(int pageNumber, int rowsPerPage)
	{
		return await _repositoryContext.Services
			.Include(x=>x.ClinicServices)
			.Skip((pageNumber - 1) * rowsPerPage)
			.Take(rowsPerPage)
			.ToListAsync();
	}

	public async Task<Service> GetServiceByID(int serviceId)
	{
		Service service = _repositoryContext.Services.Where(t=>t.ServiceId==serviceId).FirstOrDefault();
		return service;
	}

	public async Task<List<Service>?> GetServicesByClinic(string clinicId, int pageNumber, int rowsPerPage)
	{
		return await _repositoryContext.ClinicServices
			.Where(cs => cs.ClinicId.Equals(Guid.Parse(clinicId)))
			.Select(cs => cs.Service)
			.Skip((pageNumber - 1) * rowsPerPage)
			.Take(rowsPerPage)
			.ToListAsync();
	}

	public async Task<List<Service>> GetServicesByClinicByFilter(string filterValue, string clinicId, int pageNumber, int rowsPerPage)
	{
		bool status = !filterValue.Equals("false");
		return await _repositoryContext.ClinicServices
			.Where(cs => cs.ClinicId.Equals(Guid.Parse(clinicId)) && 
			             cs.Status == status )
			.Select(cs => cs.Service)
			.Skip((pageNumber - 1) * rowsPerPage)
			.Take(rowsPerPage)
			.ToListAsync();
	}
	public async Task<List<Service>> GetServicesByClinicByFilterNoPaging(string filterValue, string clinicId)
	{
		bool status = !filterValue.Equals("false");
		return await _repositoryContext.ClinicServices
			.Where(cs => cs.ClinicId.Equals(Guid.Parse(clinicId)) && 
			             cs.Status == status )
			.Select(cs => cs.Service)
			.ToListAsync();
	}

	public async Task<List<Service>> GetAllServiceNoPaging()
	{
		List<Service> models = await _repositoryContext.Services
			.Include(x => x.ClinicServices)
			.ToListAsync();
		return models;
	}

	public async Task<List<OrderService>> GetServiceUsedInDateRange(DateOnly DateStart, DateOnly DateEnd)
	{
		return await _repositoryContext.OrderServices
			.Include(os => os.Order)
			.Where(os => os.Status == 1 &&
			             DateOnly.FromDateTime(os.Order.DateTime.Value.Date) >= DateStart && 
			             DateOnly.FromDateTime(os.Order.DateTime.Value.Date) <= DateEnd)
			.ToListAsync();
	}
}