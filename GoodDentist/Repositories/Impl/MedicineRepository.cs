using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Repositories.Impl
{
    public class MedicineRepository : RepositoryBase<Medicine>, IMedicineRepository
    {
        public MedicineRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<List<Medicine>?> GetAllMedicine(int pageNumber, int pageSize)
        {
            List<Medicine> medicines = await Paging(pageNumber, pageSize);
            return medicines;
        }

        

        public async Task<Medicine?> GetMedicineByID(int Id)
        {
            return await _repositoryContext.Medicines.FirstOrDefaultAsync(c => c.MedicineId == Id);
        }

        public async Task<List<Medicine>> GetAllMedicines()
        {
            return await _repositoryContext.Medicines.ToListAsync();
        }
    }
}
