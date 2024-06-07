using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class DentistSlotRepo : RepositoryBase<DentistSlot>, IDentistSlotRepo
    {
        public DentistSlotRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<DentistSlot>?> GetAllDentistSlots(int pageNumber, int rowsPerPage)
        {
            List<DentistSlot> dentistSlots = await Paging(pageNumber, rowsPerPage);
            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage)
        {
            List<DentistSlot> dentistSlots = await Paging(pageNumber, rowsPerPage);
            dentistSlots.Where(dl => dl.DentistId.Equals(dentistId)).ToList();
            return dentistSlots;
        }

        public async Task<DentistSlot?> GetDentistSlotByID(int Id)
        {
            return await _repositoryContext.DentistSlots
                .Include(dl => dl.Room)
                .FirstOrDefaultAsync(dl => dl.DentistSlotId == Id);
        }
    }
}
