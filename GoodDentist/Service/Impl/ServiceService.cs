using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ServiceDTOs;
using BusinessObject.DTO.ServiceDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl
{
    public class ServiceService : IServiceService
	{
		private readonly IMapper mapper;
		private readonly IUnitOfWork unitOfWork;
		public ServiceService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}
		private ResponseDTO validateService(CreateServiceDTO serviceDTO)
		{

			if (serviceDTO.ServiceName.IsNullOrEmpty())
			{
				return new ResponseDTO("ServiceNam cannot be emty", 400, false, null);
			}
			return new ResponseDTO("Sucessfully!", 200, true, null);
		}

		public async Task<ResponseDTO> createService(CreateServiceDTO serviceDTO)
		{
			try
			{
				var responseDTO = validateService(serviceDTO);
				if (!responseDTO.IsSuccess)
				{
					return responseDTO;
				}
				Service service = mapper.Map<Service>(serviceDTO);
				var create = await unitOfWork.serviceRepo.CreateAsync(service);
				if (create)
				{
					return new ResponseDTO("Sucessfully", 200, true, mapper.Map<ServiceDTO>(service));
				}
				else
				{
					return new ResponseDTO("Unsucessfully", 500, false, null);
				}

			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> updateService(CreateServiceDTO model)
		{
			Service ser = await unitOfWork.serviceRepo.GetServiceByID(model.ServiceId);
			if (ser == null)
			{
				return new ResponseDTO("Cannot find the service", 400, false, null);
			}
			try
			{
				int serID=model.ServiceId;
				unitOfWork.serviceRepo.Detach(ser);
				CreateServiceDTO createServiceDTO = new CreateServiceDTO();
				createServiceDTO.ServiceName = model.ServiceName;
				createServiceDTO.Description = model.Description;
				createServiceDTO.Price = model.Price;
				createServiceDTO.Status = model.Status;
				var responseDTO = validateService(createServiceDTO);
				if (!responseDTO.IsSuccess)
				{
					return responseDTO;
				}
				Service service = mapper.Map<Service>(createServiceDTO);
				service.ServiceId = serID;
				unitOfWork.serviceRepo.Attach(service);
				var update = await unitOfWork.serviceRepo.UpdateAsync(service);
				if (!update)
				{
					return new ResponseDTO("Failed to update", 500, false, null);
					
				}
				return new ResponseDTO("Successfully", 200, true, mapper.Map<ServiceDTO>(service));
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}


		public async Task<ResponseDTO> deleteService(int serviceId)
		{
			Service ser = await unitOfWork.serviceRepo.GetServiceByID(serviceId);
			if (ser == null)
			{
				return new ResponseDTO("Cannot find the service", 400, false, null);
			}
			try
			{
				unitOfWork.serviceRepo.Detach(ser);
				CreateServiceDTO createServiceDTO = new CreateServiceDTO();
				createServiceDTO.ServiceId = ser.ServiceId;
				createServiceDTO.Price = ser.Price;
				createServiceDTO.ServiceName = ser.ServiceName;
				createServiceDTO.Description = ser.Description;	
				createServiceDTO.Status=false;
				Service service = mapper.Map<Service>(createServiceDTO);
				unitOfWork.serviceRepo.Attach(service);
				var update = await unitOfWork.serviceRepo.UpdateAsync(service);
				if (!update)
				{
					return new ResponseDTO("Failed to delete", 500, false, null);

				}
				return new ResponseDTO("Successfully", 200, true, mapper.Map<ServiceDTO>(service));
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> GetAllServices(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField,
			string? sortOrder)
		{
			try
	        {
	            List<Service> models = await unitOfWork.serviceRepo.GetAllService(pageNumber, rowsPerPage);
	                
	            // Filter 
	            if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
	            {

		            models = models;

	            }
	            else
	            {
		            switch (filterField.ToLower())
		            {
			            case "name":
				            models = models.Where(u => u.ServiceName.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
				            break;
			            case "clinicId":
				            if (Guid.TryParse(filterValue, out Guid clinicId))
				            {
					            models = models.Where(u => u.ClinicServices.Any(x => x.ClinicId == clinicId)).ToList();
				            }
				            else
				            {
					            return new ResponseDTO("Invalid Clinic ID format!", 400, false, null);
				            }
				            break;
		            }
	            }
	            // Sort
	            if (string.IsNullOrEmpty(sortField) || string.IsNullOrEmpty(sortOrder))
	            {
		            models = models;
	            }
	            else
	            {
		            bool isAscending = sortOrder.ToLower() == "asc";
		            switch (sortField.ToLower())
		            {
			            case "name":
				            models = isAscending ? models.OrderBy(u => u.ServiceName).ToList() : models.OrderByDescending(u => u.ServiceName).ToList();
				            break;
			            case "clinicId":
				            models = isAscending 
					            ? models.OrderBy(u => u.ClinicServices.Any(x => x.ClinicId == Guid.Parse(filterValue))).ToList()
					            : models.OrderByDescending(u => u.ClinicServices.Any(x => x.ClinicId == Guid.Parse(filterValue))).ToList();
				            break;
		            }
	            }
	            List<ServiceDTO> viewModels = mapper.Map<List<ServiceDTO>>(models);
	            
	            return new ResponseDTO("Get services successfully!", 200, true, viewModels);
	        }
	        catch (Exception ex)
	        {
	            return new ResponseDTO(ex.Message, 500, false, null);
	        }
		}
        

		public async Task<ResponseDTO> GetAllServicesByClinic(string clinicId, string? filterField, string? filterValue, int? pageNumber,
			int? rowsPerPage)
		{
			ResponseDTO responseDto = new ResponseDTO("", 200, true, null);
			try
			{
				if (clinicId.IsNullOrEmpty())
				{
					return AddError("Clinic Id is null!",400);
				}

				Clinic? clinic = await unitOfWork.clinicRepo.GetByIdAsync(Guid.Parse(clinicId));
				if (clinic == null)
				{
					return AddError("Clinic is not found!",404);
				}

				List<Service> services = new List<Service>();
				
				if (!filterField.IsNullOrEmpty())
				{
					if (filterField.ToLower().Equals("status"))
					{
						services = await unitOfWork.serviceRepo.GetServicesByClinicByFilter(filterValue, clinicId,
							pageNumber.Value, rowsPerPage.Value);
					}
				}
				else
				{
					services = await unitOfWork.serviceRepo.GetServicesByClinic(clinicId, pageNumber.Value, rowsPerPage.Value);

				}				
				services = await Filter(services, filterField, filterValue);

				List<ServiceDTO> serviceDtos = mapper.Map<List<ServiceDTO>>(services);

				responseDto.Result = serviceDtos;
			}
			catch (Exception e)
			{
				responseDto = AddError(e.Message, 500);
			}
			return responseDto;
		}


		public async Task<ResponseDTO> getAllService(int pageNumber, int rowsPerPage)
		{
			try
			{
				List<Service> serviceList = await unitOfWork.serviceRepo.GetAllService(pageNumber, rowsPerPage); //

				List<Service> services = mapper.Map<List<Service>>(serviceList);

				return new ResponseDTO("Get services successfully!", 200, true, services);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<List<Service>> Filter(List<Service> services, string? filterField, string? filterValue)
		{
			if (filterField.IsNullOrEmpty() || filterValue.IsNullOrEmpty())
			{
				return services;
			}

			if (services.IsNullOrEmpty())
			{
				return services;
			}

			switch (filterField.ToLower())
			{
				case "servicename":
					services = services.Where(s => s.ServiceName.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
					break;
				case "price":
					decimal number;
					if (decimal.TryParse(filterValue, out number))
					{
						services = services.Where(s => s.Price.Value.Equals(decimal.Parse(filterValue))).ToList();
					}
					break;
				default:
					return services;
			}
			return services;
		}
		
		private ResponseDTO AddError(string message, int statusCode)
		{
			ResponseDTO responseDTO = new ResponseDTO(message, statusCode, false, null);
			return responseDTO;
		}
	}
}
