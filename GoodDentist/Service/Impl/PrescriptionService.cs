using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using StackExchange.Redis;

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
				List<Prescription>? prescriptionList = await _unitOfWork.prescriptionRepo.GetPrescriptions(pageNumber, pageSize);
				

				List<PrescriptionDTO> presctiptionDTOList = _mapper.Map<List<PrescriptionDTO>>(prescriptionList);
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
			try
			{
				var prescription = await _unitOfWork.prescriptionRepo.GetByIdAsync(prescriptionId);
				if (prescription == null||prescription.Status == false)
				{
					return new ResponseDTO("This prescription is not exist!", 400, false, null);
				}
				prescription.Status = false;
				var result = await _unitOfWork.prescriptionRepo.DeleteAsync(prescription);
				if (result)
				{
					return new ResponseDTO("Prescription Delete succesfully!", 201, true, null);
				}
				return new ResponseDTO("Prescription Delete unsucessfully!", 400, false, null);
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> AddPrescription(PrescriptionCreateDTO prescriptionDTO)
		{
			try
			{
				/*var check = await CheckValidationAddPrescription(prescriptionDTO);
				if (check.IsSuccess == false)
				{
					return check;
				}*/
				
				Prescription prescription = _mapper.Map<Prescription>(prescriptionDTO);
				_unitOfWork.prescriptionRepo.CreatePrescription(prescription);
				
				if(!prescriptionDTO.Medicines.IsNullOrEmpty())
				{
					foreach (var medicineDto in prescriptionDTO.Medicines)
					{
						if(_unitOfWork.medicineRepo.GetMedicineByID(medicineDto.MedicineId).Result != null )
						{
							MedicinePrescription medicinePrescription = new MedicinePrescription
							{
								
								Prescription = prescription,
								PrescriptionId = prescription.PrescriptionId,
								Medicine = _mapper.Map<Medicine>(medicineDto),
								MedicineId = medicineDto.MedicineId,
								Quantity = medicineDto.Quantity,
								Price = medicineDto.Price*medicineDto.Quantity,
								Status = true,

							};
							prescription.MedicinePrescriptions.Add(medicinePrescription);
						}
					}
					
				}
				
				
				await _unitOfWork.prescriptionRepo.UpdatePresription(prescription);
				return new ResponseDTO("Create successfully", 200, true, _mapper.Map<PrescriptionDTO>(prescription));
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> UpdatePrescription(PrescriptionDTO prescriptionDTO)
		{
			try
			{
				var prescription = await _unitOfWork.prescriptionRepo.GetByIdAsync(prescriptionDTO.PrescriptionId);
				if (prescription == null || prescription.Status == false)
				{
					return new ResponseDTO("This prescription is not exist!", 400, false, null);
				}
				var check = await CheckValidationUpdatePrescription(prescriptionDTO);
				if (check.IsSuccess == false)
				{
					return check;
				}
				int prescriptionId = prescription.PrescriptionId;

				_unitOfWork.prescriptionRepo.Detach(prescription);

				var prescriptionUpdate = _mapper.Map<Prescription>(prescriptionDTO);

				prescriptionUpdate.PrescriptionId = prescriptionId;

				_unitOfWork.prescriptionRepo.Attach(prescriptionUpdate);

				await _unitOfWork.prescriptionRepo.UpdateAsync(prescriptionUpdate);
				return new ResponseDTO("Update Sucessfully!", 201, true, null);
				
			}
			catch (Exception ex)
			{
				return new ResponseDTO (ex.Message, 500, false, null);
			}
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
				return new ResponseDTO("Prescription's price must be greater than 0", 400, false, null);
			}
			if(prescriptionDTO.Total == null)
			{
				return new ResponseDTO("Please input presrciption's price", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}

		public async Task<ResponseDTO> CheckValidationUpdatePrescription(PrescriptionDTO prescriptionDTO)
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
				return new ResponseDTO("Prescription's price must be greater than 0!", 400, false, null);
			}

			if(prescriptionDTO.Total == null)
			{
				return new ResponseDTO("Please input presription's price", 400, false, null);
			}

			if(prescriptionDTO.Total > decimal.MaxValue)
			{
				return new ResponseDTO("Prescription value is out of range!", 400, false, null);
			}
			return new ResponseDTO("Check validation successfully", 200, true, null);
		}
	}
}
