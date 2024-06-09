using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Repositories.Impl;

namespace Services.Impl
{
    public class MedicineService : IMedicineService
    {
        
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MedicineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ResponseDTO> GetAllMedicine(int pageNumber, int pageSize)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<Medicine>? medicineList = await unitOfWork.medicineRepo.GetAllMedicine(pageNumber, pageSize);
                var all = medicineList.Where(c => c.Status == true);

                List<MedicineUpdateDTO> medicineDTOList = mapper.Map<List<MedicineUpdateDTO>>(all);

                responseDTO.Message = "Get all Medicine successfully!";
                responseDTO.Result = medicineDTOList;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message = ex.Message;
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> SearchMedicine(string searchValue)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<Medicine>? medicinesList = await unitOfWork.medicineRepo.FindByConditionAsync(c => c.MedicineName == searchValue);
                var all = medicinesList.Where(c => c.Status == true);
                List<MedicineDTO> medicineDTOList = mapper.Map<List<MedicineDTO>>(all);

                responseDTO.Message = "Search Medicine successfully!";
                responseDTO.Result = medicineDTOList;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message = ex.Message;
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> DeleteMedicine(int medicineId)
        {
            try
            {
                var medicine = await unitOfWork.medicineRepo.GetByIdAsync( medicineId);
                if (medicine == null)
                {
                    return new ResponseDTO("This medicine is not exist!", 400, false, null);
                }
                medicine.Status = false;
                var result = await unitOfWork.medicineRepo.DeleteAsync(medicine);
                if (result)
                {
                    return new ResponseDTO("Medicine Delete succesfully!", 201, true, null);
                }
                return new ResponseDTO("Medicine Delete unsucessfully!", 400, false, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        

        public async Task<ResponseDTO> AddMedicine(MedicineDTO medicineDTO)
        {
            
            try
            {
                var check = await CheckValidationAddMedicine(medicineDTO);
                if (check.IsSuccess == false)
                {
                    return check;
                }

                Medicine medicine = mapper.Map<Medicine>(medicineDTO);
                await unitOfWork.medicineRepo.CreateAsync(medicine);
                return new ResponseDTO("Creat succesfully", 200, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        public async Task<ResponseDTO> UpdateMedicine(MedicineUpdateDTO medicineDTO)
        {
            try
            {
                var medicine = await unitOfWork.medicineRepo.GetByIdAsync(medicineDTO.MedicineId);
                if (medicine == null)
                {
                    return new ResponseDTO("This medicine is not exist!", 400, false, null);
                }
                var check = await CheckValidationUpdateMedicine(medicineDTO);
                if (check.IsSuccess == false)
                {
                    return check;
                }
                int medicineId = medicine.MedicineId;

                unitOfWork.medicineRepo.Detach(medicine);

                var updateMedicine = mapper.Map<Medicine>(medicineDTO);

                updateMedicine.MedicineId = medicineId;

                unitOfWork.medicineRepo.Attach(updateMedicine);

                await unitOfWork.medicineRepo.UpdateAsync(updateMedicine);
                return new ResponseDTO("Update Sucessfully!", 201, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
            
        }

        public async Task<ResponseDTO> CheckValidationAddMedicine(MedicineDTO medicineDTO)
        {

            if (medicineDTO.MedicineName.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine name",400, false, null);
            }

            if (medicineDTO.Type.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine type", 400, false, null);
            }

            if (medicineDTO.Quantity.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine quantity", 400, false, null);
            }

            if (medicineDTO.Description.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine description", 400, false, null);
            }

            if (medicineDTO.Price.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine price", 400, false, null);
            }

            List<Medicine> medicineList = await unitOfWork.medicineRepo.FindByConditionAsync(c => c.Status == true);
            if (medicineList.Any(c => c.MedicineName == medicineDTO.MedicineName))
            {
                return new ResponseDTO("Medicine name is already existed!", 400, false, null);
            }

            if (medicineDTO.Quantity < 0)
            {
                return new ResponseDTO("Please input correct medicine quantity (quantity must be greater or equal to 0)", 400, false, null);
            }

            if (medicineDTO.Price < 0)
            {
                return new ResponseDTO("Please input correct medicine price (price must be greater or equal to 0", 400, false, null);
            }
            return new ResponseDTO("Check validation successfully",200,  true, null);
        }

        public async Task<ResponseDTO> CheckValidationUpdateMedicine(MedicineUpdateDTO medicineDTO)
        {
            if (medicineDTO.MedicineId.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine id", 400, false, null);
            }
            if (medicineDTO.MedicineName.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine name", 400, false, null);
            }

            if (medicineDTO.Type.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine type", 400, false, null);
            }

            if (medicineDTO.Quantity.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine quantity", 400, false, null);
            }

            if (medicineDTO.Description.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine description", 400, false, null);
            }

            if (medicineDTO.Price.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine price", 400, false, null);
            }

            List<Medicine> medicineList = await unitOfWork.medicineRepo.FindByConditionAsync(c => c.Status == true);
            if (medicineList.Any(c => c.MedicineName == medicineDTO.MedicineName&& c.MedicineId != medicineDTO.MedicineId))
            {
                return new ResponseDTO("Medicine name is already existed!", 400, false, null);
            }

            if (medicineDTO.Quantity < 0)
            {
                return new ResponseDTO("Please input correct medicine quantity (quantity must be greater or equal to 0)", 400, false, null);
            }

            if (medicineDTO.Price < 0)
            {
                return new ResponseDTO("Please input correct medicine price (price must be greater or equal to 0", 400, false, null);
            }
            return new ResponseDTO("Check validation successfully", 200, true, null);
        }
    }
}
