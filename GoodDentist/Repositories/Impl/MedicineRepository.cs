using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories.Impl
{
    public class MedicineRepository : IMedicineRepository
    {
        public List<Medicine> GetAllMedicine()
        {
            throw new NotImplementedException();
        }

        public List<Medicine> SearchMedicine(string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMedicine(int medicintId)
        {
            throw new NotImplementedException();
        }

        public bool AddMedicine(string medicineName, string type, int quantity, string description, decimal price)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMedicine(int medicineId, string medicineName, string type, int quantity, string description, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
