using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.MedicineDTOs;
using BusinessObject.DTO.MedicineDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs.View;
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
        public async Task<ResponseDTO> GetAllMedicine(string? filterField, string? filterValue, string? sortField,
            string? sortValue, string? search,int? pageNumber, int? pageSize)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<Medicine>? medicineList = await unitOfWork.medicineRepo.GetAllMedicines();
                medicineList = await FilterMedicine(medicineList, filterField, filterValue);
                medicineList = await Search(medicineList, search);
                medicineList = await SortMedicine(medicineList, sortField, sortValue);
                medicineList = Paging(medicineList, pageNumber, pageSize);
                
                List<MedicineDTO> medicineDTOList = mapper.Map<List<MedicineDTO>>(medicineList);

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
                if (medicine == null || medicine.Status == false)
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

        public async Task<ResponseDTO> UpdateMedicineAfterPaymentPrescription(int prescriptionId)
        {
            try
            {
                Prescription? prescription = await unitOfWork.prescriptionRepo.GetPrescriptionById(prescriptionId);
                if (prescription != null)
                {
                    List<MedicinePrescription> medicinePrescriptions = prescription.MedicinePrescriptions.ToList();
                    // check input medicines
                    if (!medicinePrescriptions.IsNullOrEmpty())
                    {
                        foreach (MedicinePrescription? medicinePrescription in medicinePrescriptions)
                        {
                            if(medicinePrescription != null)
                            {
                                if (medicinePrescription.MedicineId != null)
                                {
                                    Medicine? medicineModel =
                                        await unitOfWork.medicineRepo.GetByIdAsync(medicinePrescription.MedicineId);
                                    // medicine in storage
                                    if (medicineModel != null)
                                    {
                                        medicineModel.Quantity -= medicinePrescription.Quantity;
                                        // check valid quantity
                                        if (medicineModel.Quantity <= 0) return new ResponseDTO("Medicine in storage is not available for this quantity!", 404, false, null);
                                        await unitOfWork.medicineRepo.UpdateAsync(medicineModel);
                                        
                                    }
                                }
                            } else return new ResponseDTO("Medicine not found!", 404, false, null);
                        }
                        // update prescription : PAID
                        prescription.Status = false;
                        
                        prescription = await unitOfWork.prescriptionRepo.UpdatePrescription(prescription);
                        return new ResponseDTO("Update Medicine Storage Successfully", 200, true, mapper.Map<PrescriptionDTO>(prescription));
                    } return new ResponseDTO("Medicines not found!", 404, false, null);
                }  return new ResponseDTO("Prescription not found!", 404, false, null);
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
                return new ResponseDTO("Create succesfully", 200, true, null);
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
                if (medicine == null || medicine.Status == false)
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

            if(medicineDTO.Quantity == null)
            {
				return new ResponseDTO("Please input medicine quantity", 400, false, null);
			}
			if (medicineDTO.Quantity > int.MaxValue)
			{
				return new ResponseDTO("Medicine's quanty is out of range!", 400, false, null);
			}

			if (medicineDTO.Price < 0)
            {
                return new ResponseDTO("Please input correct medicine price (price must be greater or equal to 0", 400, false, null);
            }

            if(medicineDTO.Price == null)
            {
				return new ResponseDTO("Please input medicine price", 400, false, null);
			}
            if(medicineDTO.Price > int.MaxValue)
            {
                return new ResponseDTO("Medicine's price is out of range!", 400, false, null);
            }
			return new ResponseDTO("Check validation successfully",200,  true, null);
        }

        public async Task<ResponseDTO> CheckValidationUpdateMedicine(MedicineUpdateDTO medicineDTO)
        {
            if (medicineDTO.MedicineId.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input medicine id", 400, false, null);
            }

            Medicine? medicine = await unitOfWork.medicineRepo.GetByIdAsync(medicineDTO.MedicineId);
            if (medicine == null)
            {
                return new ResponseDTO("Medicine is not exist!", 404, false, null);

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

            if(medicineDTO.Quantity == null)
            {
                return new ResponseDTO("Please input medicine's quantity!", 400, false, null);
            }

            if(medicineDTO.Quantity > int.MaxValue)
            {
                return new ResponseDTO("Medicine's quanty is out of range!", 400 , false, null);
            }

            if (medicineDTO.Price < 0)
            {
                return new ResponseDTO("Please input correct medicine price (price must be greater or equal to 0", 400, false, null);
            }

			if (medicineDTO.Price == null)
			{
				return new ResponseDTO("Please input medicine price", 400, false, null);
			}

            if(medicineDTO.Price > decimal.MaxValue)
            {
                return new ResponseDTO("Medicine's price is out of range!", 400, false, null);
            }

            if (medicineDTO.Unit.IsNullOrEmpty())
            {
                return new ResponseDTO("Unit is empty!", 400, false, null);
            }

            if (!medicineDTO.Status.HasValue)
            {
                return new ResponseDTO("Please input status", 400, false, null);
            }
			return new ResponseDTO("Check validation successfully", 200, true, null);
        }
        
        private async Task<List<Medicine>> FilterMedicine (List<Medicine> medicines,string? filterField, string? filterValue)
        {
            if (filterField.IsNullOrEmpty() || filterValue.IsNullOrEmpty())
            {
                return medicines;
            }

            switch (filterField.ToLower())
            {
                case "medicinename":
                    return medicines.Where(m => m.MedicineName.Contains(filterValue)).ToList();
                case "type":
                    return medicines.Where(m => m.Type.Contains(filterValue)).ToList();
                case "unit":
                    return medicines.Where(m => m.Unit.Contains(filterValue)).ToList();
                case "description":
                    return medicines.Where(m => m.Description.Contains(filterValue)).ToList();
                case "status":
                    bool s = true;
                    if (filterValue.Equals("false")) s = false;
                    return medicines.Where(m => m.Status == s).ToList();
                case "price":
                    return medicines.Where(m => m.Price.ToString().Equals(filterValue)).ToList();
            }
            return medicines;
        }

        private async Task<List<Medicine>> Search(List<Medicine> medicines, string? search)
        {
            if (search.IsNullOrEmpty())
            {
                return medicines;
            }

            return medicines.Where(m => m.MedicineName.Contains(search, StringComparison.OrdinalIgnoreCase) 
                                 || m.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
                                 ).ToList();
        }

        private async Task<List<Medicine>> SortMedicine(List<Medicine> medicines ,string? sortField, string? sortValue)
        {
            if (sortField.IsNullOrEmpty())
            {
                return medicines;
            }

            bool isAscending = sortValue.ToLower().Equals("asc");

            switch (sortField.ToLower())
            {
                case "medicinename":
                    return isAscending
                        ? medicines.OrderBy(m => m.MedicineName).ToList()
                        : medicines.OrderByDescending(m => m.MedicineName).ToList();
                case "price":
                    return isAscending
                        ? medicines.OrderBy(m => m.Price).ToList()
                        : medicines.OrderByDescending(m => m.Price).ToList();
                case "unit":
                    return isAscending
                        ? medicines.OrderBy(m => m.Unit).ToList()
                        : medicines.OrderByDescending(m => m.Unit).ToList();
                case "quantity":
                    return isAscending
                        ? medicines.OrderBy(m => m.Quantity).ToList()
                        : medicines.OrderByDescending(m => m.Quantity).ToList();
                case "type":
                    return isAscending
                        ? medicines.OrderBy(m => m.Type).ToList()
                        : medicines.OrderByDescending(m => m.Type).ToList();
            }
            return medicines;
        }

        private List<Medicine> Paging(List<Medicine> medicines, int? pageNumber, int? pageSize)
        {
           return medicines.Skip((pageNumber.Value -1 ) * pageSize.Value).Take(pageSize.Value).ToList();
        }
    }
}
