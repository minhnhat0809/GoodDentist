using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.PrescriptionDTOs;
using BusinessObject.DTO.PrescriptionDTOs.View;
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

        public async Task<ResponseDTO> GetPrescriptionDetails(int prescriptionId)
        {
            ResponseDTO responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                if (prescriptionId <= 0)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Prescription Id is null!";
                    responseDto.StatusCode = 400;
                    return responseDto;
                }

                Prescription? prescription = await _unitOfWork.prescriptionRepo.GetPrescriptionById(prescriptionId);
                if (prescription == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Prescription is not found!";
                    responseDto.StatusCode = 404;
                    return responseDto;
                }

                PrescriptionDTO prescriptionDto = _mapper.Map<PrescriptionDTO>(prescription);

                responseDto.Message = "Get successfully!";
                responseDto.Result = prescriptionDto;
            }
            catch (Exception e)
            {
                responseDto.Message = e.Message;
                responseDto.IsSuccess = false;
                responseDto.StatusCode = 500;
            }
            return responseDto;
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
				prescription.Total = 0;
				if(!prescriptionDTO.Medicines.IsNullOrEmpty())
				{
					foreach (var medicineDto in prescriptionDTO.Medicines)
					{
						var medicine = await _unitOfWork.medicineRepo.GetByIdAsync(medicineDto.MedicineId);
						if(medicine != null)
						{
							// check Medicine quantity is valid? 
							MedicinePrescription medicinePrescription = new MedicinePrescription
							{

								MedicineId = medicine.MedicineId,
								Quantity = medicineDto.MedicineId,
								Price = medicine.Price * medicineDto.Quantity,
								Status = true
								
							};
							prescription.MedicinePrescriptions.Add(medicinePrescription);
							prescription.Total += medicinePrescription.Price;
						}
					}
				}

				if (prescriptionDTO.ExaminationId != null)
				{
					prescription.Examination =
						await _unitOfWork.examinationRepo.GetExaminationById(prescriptionDTO.ExaminationId.Value);
				}
				await _unitOfWork.prescriptionRepo.CreatePrescription(prescription);
				return new ResponseDTO("Create successfully", 200, true, _mapper.Map<PrescriptionDTO>(prescription));
			}
			catch (Exception ex)
			{
				return new ResponseDTO(ex.Message, 500, false, null);
			}
		}

		public async Task<ResponseDTO> UpdatePrescription(PrescriptionUpdateDTO prescriptionDTO)
		{
			try
			{
				Prescription prescription = await _unitOfWork.prescriptionRepo.GetByIdAsync(prescriptionDTO.PrescriptionId);
				if (prescription == null || prescription.Status == false)
				{
					return new ResponseDTO("This prescription is not exist!", 400, false, null);
				}
				/*var check = await CheckValidationUpdatePrescription(prescriptionDTO);
				if (check.IsSuccess == false)
				{
					return check;
				}*/
				prescription = _mapper.Map<Prescription>(prescriptionDTO);
				prescription.Total = 0;
				if(!prescriptionDTO.Medicines.IsNullOrEmpty())
				{
					foreach (var medicineDto in prescriptionDTO.Medicines)
					{
						var medicine = await _unitOfWork.medicineRepo.GetByIdAsync(medicineDto.MedicineId);
						if(medicine != null)
						{
							// check Medicine quantity is valid? 
							MedicinePrescription medicinePrescription = new MedicinePrescription
							{

								MedicineId = medicine.MedicineId,
								Quantity = medicineDto.MedicineId,
								Price = medicine.Price * medicineDto.Quantity,
								Status = true
								
							};
							prescription.MedicinePrescriptions.Add(medicinePrescription);
							prescription.Total += medicinePrescription.Price;
						}
					}
				}

				if (prescriptionDTO.ExaminationId != null)
				{
					prescription.Examination =
						await _unitOfWork.examinationRepo.GetExaminationById(prescriptionDTO.ExaminationId.Value);
				}
				await _unitOfWork.prescriptionRepo.UpdatePrescription(prescription);
				return new ResponseDTO("Update Successfully!", 201, true, _mapper.Map<PrescriptionDTO>(prescription));
				
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
