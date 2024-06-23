using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories;

namespace Services.Impl
{
	public class ClinicServiceService : IClinicServiceService
	{
		private readonly IMapper mapper;
		private readonly IUnitOfWork unitOfWork;
		public ClinicServiceService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			this.mapper = mapper;
			this.unitOfWork = unitOfWork;
		}


		public async Task<ResponseDTO> CreateClinicService(ClinicServiceDTO clinicServiceDTO)
		{
			try
			{
				ResponseDTO validate = await CheckValidate(clinicServiceDTO);
				if (validate.IsSuccess)
				{
					BusinessObject.Entity.ClinicService clinicService = mapper.Map<BusinessObject.Entity.ClinicService>(clinicServiceDTO);
					var create = await unitOfWork.clinicServiceRepo.CreateAsync(clinicService);
					if (create)
					{
						return new ResponseDTO("Success", 200, true, null);
					}
				}
				else
				{
					return new ResponseDTO(validate.Message, 400, false, null);
				}

			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
			return new ResponseDTO("Failed to create", 500, false, null);
		}

		private async Task<ResponseDTO> CheckValidate(ClinicServiceDTO clinicServiceDTO)
		{
			ResponseDTO responseDTO = await unitOfWork.clinicServiceRepo.CheckValidate(clinicServiceDTO);
			return responseDTO;
		}

		public async Task<ResponseDTO> getAllClinicService(int pageNumber, int rowsPerPage)
		{
			try
			{
				List<BusinessObject.Entity.ClinicService> clinicServices = await unitOfWork.clinicServiceRepo.GetAllClinicService(pageNumber, rowsPerPage);
				List<BusinessObject.Entity.ClinicService> list = mapper.Map<List<BusinessObject.Entity.ClinicService>>(clinicServices);
				return new ResponseDTO("Get clinic service successfully!", 200, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> UpdateClinicService(ClinicServiceDTO model)
		{
			BusinessObject.Entity.ClinicService clinicService = await unitOfWork.clinicServiceRepo.GetClinicServiceByID(model.ClinicServiceId);
			if (clinicService == null)
			{
				return new ResponseDTO("Cannot find the clinicService", 400, false, null);
			}
			try
			{
				int clinicServiceId = model.ClinicServiceId;
				unitOfWork.clinicServiceRepo.Detach(clinicService);
				ClinicServiceDTO clinicServiceDTO = new ClinicServiceDTO();
				clinicServiceDTO.ClinicId = model.ClinicId;
				clinicServiceDTO.ServiceId = model.ServiceId; ;
				clinicServiceDTO.Price = model.Price;
				BusinessObject.Entity.ClinicService cs = mapper.Map<BusinessObject.Entity.ClinicService>(clinicServiceDTO);
				cs.ClinicServiceId = clinicServiceId;
				cs.Status = true;
				unitOfWork.clinicServiceRepo.Attach(cs);
				var update = await unitOfWork.clinicServiceRepo.UpdateAsync(cs);
				if (!update)
				{
					return new ResponseDTO("Failed to update", 500, false, null);

				}
				return new ResponseDTO("Sucessfully", 200, true, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}

		}
	}
}
