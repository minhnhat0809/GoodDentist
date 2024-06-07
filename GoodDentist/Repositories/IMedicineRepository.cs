using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories
{
    public interface IMedicineRepository
    {
        List<Medicine> GetAllMedicine();

        List<Medicine> SearchMedicine(string searchValue);

        bool AddMedicine(string medicineName, string type, int quantity,
            string description, decimal price);

        bool UpdateMedicine(int medicineId, string medicineName, string type, int quantity,
            string description, decimal price);

        bool DeleteMedicine(int medicintId);
    }
}
