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
            List<DentistSlot> dentistSlots = await _repositoryContext.DentistSlots
                .Include(dl => dl.Room)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync(); ;
            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage)
        {
            List<DentistSlot> dentistSlots = await Paging(pageNumber, rowsPerPage);
            dentistSlots.Where(dl => dl.DentistId.Equals(dentistId)).ToList();
            return dentistSlots;
        }

        public async Task<DentistSlot?> GetDentistSlotByDentistAndTimeStart(string dentistId, DateTime timeStart)
        {
            List<DentistSlot> dentistSlots = await FindByConditionAsync(dl => dl.DentistId.Equals(Guid.Parse(dentistId)) && dl.TimeStart.Equals(timeStart));
            return dentistSlots.FirstOrDefault();
        }

        public async Task<DentistSlot?> GetDentistSlotByID(int Id)
        {
            return await _repositoryContext.DentistSlots
                .Include(dl => dl.Room)
                .FirstOrDefaultAsync(dl => dl.DentistSlotId == Id);
        }

        public async Task<DentistSlot?> GetDentistSlotsByRoomAndTimeStart(int roomId, DateTime timeStart)
        {
            List<DentistSlot> dentistSlots = await FindByConditionAsync(dl => dl.RoomId == roomId && dl.TimeStart.Equals(timeStart));
            return dentistSlots.FirstOrDefault();
        }
    }
}
