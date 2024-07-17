using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.MedicineDTOs;
using BusinessObject.DTO.MedicineDTOs.View;
using BusinessObject.Entity;

namespace Services
{
    public interface IMedicineService
    {
        Task<ResponseDTO> GetAllMedicine(string? filterField, string? filterValue, string? sortField,
            string? sortValue, string? search,int? pageNumber, int? pageSize);

        Task<ResponseDTO> SearchMedicine(string searchValue);

        Task<ResponseDTO> AddMedicine(MedicineDTO medicineDTO);

        Task<ResponseDTO> UpdateMedicine(MedicineUpdateDTO medicineDTO);

        Task<ResponseDTO> DeleteMedicine(int medicineId);
        
        Task<ResponseDTO> UpdateMedicineAfterPaymentPrescription(int prescriptionId);
    }
}
