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
					return new ResponseDTO("Sucessfully", 200, true, null);
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
				return new ResponseDTO("", 400, false, null);
			}
			try
			{
				int serID=model.ServiceId;
				unitOfWork.serviceRepo.Detach(ser);
				CreateServiceDTO createServiceDTO = new CreateServiceDTO();
				createServiceDTO.ServiceName = model.ServiceName;
				createServiceDTO.Description = model.Description;
				createServiceDTO.Price = model.Price;
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
					return new ResponseDTO("", 500, false, null);
					
				}
				return new ResponseDTO("", 200, true, null);
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
				return new ResponseDTO("", 400, false, null);
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
					return new ResponseDTO("", 500, false, null);

				}
				return new ResponseDTO("", 200, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
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
	}
}
