using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl
{
	public class PrescriptionService : IPrescriptionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		public async Task<ResponseDTO> GetAllPrescription(int pageNumber, int pageSize)
		{
			try
			{
				List<Prescription>? prescriptionList = await _unitOfWork.prescriptionRepo.GetAllPrescription(pageNumber, pageSize);
				var all = prescriptionList.Where(c => c.Status == true);

				List<PrescriptionDTO> presctiptionDTOList = _mapper.Map<List<PrescriptionDTO>>(all);
				return new ResponseDTO("Get all Order successfully!", 200, true, presctiptionDTOList);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> SearchPrescription(string searchValue)
		{
			try
			{
				List<Prescription>? prescriptionList = await _unitOfWork.prescriptionRepo.FindByConditionAsync(c => c.PrescriptionId.ToString() == searchValue);
				var all = prescriptionList.Where(c => c.Status == true);
				List<PrescriptionDTO> prescriptionDTOList = _mapper.Map<List<PrescriptionDTO>>(all);
				if (prescriptionDTOList.IsNullOrEmpty())
				{
					return new ResponseDTO("No result found!", 200, true, null);
				}

				return new ResponseDTO("Search Prescription successfully!", 200, true, prescriptionDTOList);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> DeletePrescription(int prescriptionId)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> AddPrescription(PrescriptionCreateDTO prescriptionDTO)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> UpdatePrescription(PrescriptionDTO prescriptionDTO)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDTO> CheckValidationAddPrescription(PrescriptionCreateDTO prescriptionDTO)
		{
			if (prescriptionDTO.DateTime.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input presrciption date time ", 400, false, null);
			}


			if (prescriptionDTO.Note.IsNullOrEmpty())
			{
				return new ResponseDTO("Please input prescription's note!", 400, false, null);
			}

			
			if (prescriptionDTO.Total < 0)
			{
				return new ResponseDTO("Order's price must be greater than 0!", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}

		public async Task<ResponseDTO> CheckValidationUpdateOrder(PrescriptionDTO prescriptionDTO)
		{
			if (prescriptionDTO.PrescriptionId.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please choose order!", 400, false, null);
			}
			if (prescriptionDTO.DateTime.ToString().IsNullOrEmpty())
			{
				return new ResponseDTO("Please input presrciption date time ", 400, false, null);
			}


			if (prescriptionDTO.Note.IsNullOrEmpty())
			{
				return new ResponseDTO("Please input prescription's note!", 400, false, null);
			}


			if (prescriptionDTO.Total < 0)
			{
				return new ResponseDTO("Order's price must be greater than 0!", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}
	}
}
