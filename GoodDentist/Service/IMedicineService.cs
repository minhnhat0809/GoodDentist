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
        List<Medicine> GetAllMedicine();

        List<Medicine> SearchMedicine(string searchValue);

        ValidationResponseDTO CheckValidationAddMedicine(string medicineName, string type, int quantity,
            string description, decimal price);

        bool AddMedicine(string medicineName, string type, int quantity, 
            string description, decimal price);

        ValidationResponseDTO CheckValidationUpdateMedicine(int medicineId, string medicineName, string type, int quantity,
            string description, decimal price);

        bool UpdateMedicine(int medicineId, string medicineName, string type, int quantity,
            string description, decimal price);

        bool DeleteMedicine(int medicineId);
    }
}
