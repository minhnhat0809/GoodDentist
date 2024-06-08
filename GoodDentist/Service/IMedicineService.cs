using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;

namespace Services
{
    public interface IMedicineService
    {
        Task<ResponseDTO> GetAllMedicine(int pageNumber, int pageSize);

        List<Medicine> SearchMedicine(string searchValue);

        Task<ResponseDTO> AddMedicine(MedicineDTO medicineDTO);

        Task<ResponseDTO> UpdateMedicine(MedicineUpdateDTO medicineDTO);

        Task<ResponseDTO> DeleteMedicine(int medicineId);
    }
}
