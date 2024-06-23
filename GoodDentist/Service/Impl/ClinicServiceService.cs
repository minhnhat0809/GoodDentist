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
		private readonly IClinicService clinicService;
		public ClinicServiceService(IMapper mapper, IUnitOfWork unitOfWork, IClinicService clinicService)
		{
			this.mapper = mapper;
			this.unitOfWork = unitOfWork;
			this.clinicService = clinicService;
		}

		{
			return clinicService;
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
				} else
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
	}
}
