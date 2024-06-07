using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Repositories.Impl;

namespace Services.Impl
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        public MedicineService()
        {
            _medicineRepository = new MedicineRepository();
        }
        public List<Medicine> GetAllMedicine()
        {
            List<Medicine> medicineList = _medicineRepository.GetAllMedicine();
            return medicineList;
        }

        public List<Medicine> SearchMedicine(string searchValue)
        {
            List<Medicine> searchList = _medicineRepository.SearchMedicine(searchValue);
            return searchList;
        }

        public bool DeleteMedicine(int medicineId)
        {
            return _medicineRepository.DeleteMedicine(medicineId);
        }

        public ValidationResponseDTO CheckValidationAddMedicine(string medicineName, string type, int quantity, string description, decimal price)
        {
            if (medicineName.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine name", false);
            }

            if (type.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine type", false);
            }

            if (quantity.ToString().IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine quantity", false);
            }

            if (description.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine description", false);
            }

            if (price.ToString().IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine price", false);
            }

            List<Medicine> medicineList = GetAllMedicine();
            if (medicineList.Any(c => c.MedicineName == medicineName))
            {
                return new ValidationResponseDTO("Medicine name is already existed!", false);
            }

            if (quantity < 0)
            {
                return new ValidationResponseDTO("Please input correct medicine quantity (quantity must be greater or equal to 0)", false);
            }

            if (price < 0)
            {
                return new ValidationResponseDTO("Please input correct medicine price (price must be greater or equal to 0", false);
            }
            return new ValidationResponseDTO("Check validation successfully", true);
        }

        public bool AddMedicine(string medicineName, string type, int quantity, string description, decimal price)
        {
            return _medicineRepository.AddMedicine(medicineName, type, quantity, description, price);
        }

        public ValidationResponseDTO CheckValidationUpdateMedicine(int medicineId, string medicineName, string type, int quantity, string description, decimal price)
        {
            if (medicineName.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine name", false);
            }

            if (type.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine type", false);
            }

            if (quantity.ToString().IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine quantity", false);
            }

            if (description.IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine description", false);
            }

            if (price.ToString().IsNullOrEmpty())
            {
                return new ValidationResponseDTO("Please input medicine price", false);
            }

            List<Medicine> medicineList = GetAllMedicine();
            if (medicineList.Any(c => c.MedicineId != medicineId && c.MedicineName == medicineName))
            {
                return new ValidationResponseDTO("Medicine name is already existed!", false);
            }

            if (quantity < 0)
            {
                return new ValidationResponseDTO("Please input correct medicine quantity (quantity must be greater or equal to 0)", false);
            }

            if(price < 0)
            {
                return new ValidationResponseDTO("Please input correct medicine price (price must be greater or equal to 0", false);
            }
            return new ValidationResponseDTO("Check validation successfully", true);
        }

        public bool UpdateMedicine(int medicineId, string medicineName, string type, int quantity, string description, decimal price)
        {
            return _medicineRepository.UpdateMedicine(medicineId, medicineName, type, quantity, description, price);
        }
    }
}
