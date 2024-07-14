using BusinessObject;
using BusinessObject.Entity;
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
                .Include(dl => dl.Dentist)
                .Include(dl => dl.Room)
                .Include(dl => dl.Examinations)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync(); ;
            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllDentistSlotsByDentistAndTimeStart(string clinicId, DateTime timeStart, DateTime timeEnd)
        {
            List<DentistSlot>? dentistSlots = new List<DentistSlot>();

            TimeSpan timecheck = new TimeSpan(0, 0, 0);
            if (timeStart.TimeOfDay == timecheck)
            {
                List<User?> users = await _repositoryContext.ClinicUsers
                    .Include(cu => cu.User)
                    .ThenInclude(u => u.DentistSlots)
                    .Where(cu => cu.ClinicId.Equals(Guid.Parse(clinicId))
                    && cu.Status == true).Select(cu => cu.User).ToListAsync();
                dentistSlots = users
                    .Where(u => u != null)
                    .SelectMany(u => u.DentistSlots)
                    .Where(dl => dl.TimeStart.Value.Date == timeStart.Date)
                    .ToList();
            }
            else
            {
                List<User?> users = await _repositoryContext.ClinicUsers
                    .Include(cu => cu.User)
                    .ThenInclude(u => u.DentistSlots).ThenInclude(dl => dl.Examinations)
                    .Include(cu => cu.User)
                    .ThenInclude(u => u.DentistSlots).ThenInclude(dl => dl.Room)
                    .Where(cu => cu.ClinicId.Equals(Guid.Parse(clinicId))
                    && cu.Status == true && cu.User.RoleId == 2).Select(cu => cu.User).ToListAsync();

                     dentistSlots = users
                    .Where(u => u != null)
                    .SelectMany(u => u.DentistSlots)
                    .Where(dl => dl.TimeStart.Value.Date == timeStart.Date
                    && (dl.TimeStart <= timeStart && timeEnd < dl.TimeEnd) || (dl.TimeStart < timeStart && timeEnd <= dl.TimeEnd)
                    && dl.Room.ClinicId.Equals(Guid.Parse(clinicId))
                    && dl.Status == true)
                    .ToList();
            }
            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllSlotsOfClinic(string clinicId, int pageNumber, int rowsPerPage)
        {
            List<DentistSlot> dentistSlots = await _repositoryContext.DentistSlots
                .Include(dl => dl.Dentist)
                .Include(dl => dl.Room)
                .Include(dl => dl.Examinations)
                .Where(dl => dl.Room.Clinic.ClinicId.Equals(Guid.Parse(clinicId)))
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();

            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage)
        {
            List<DentistSlot> dentistSlots = await _repositoryContext.DentistSlots
                .Include(dl => dl.Dentist).ThenInclude(d => d.ClinicUsers)
                .Include(dl => dl.Room)
                .Include(dl => dl.Examinations)
                .Where(dl => dl.DentistId.Equals(Guid.Parse(dentistId)) &&
                 dl.Room.ClinicId.Equals(dl.Dentist.ClinicUsers
                     .FirstOrDefault(cu => cu.Status == true && cu.UserId.Equals(dl.DentistId)).ClinicId))
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
            return dentistSlots;
        }

        public async Task<List<DentistSlot>?> GetAllSlotsOfDentistByDate(string clinicId, string dentistId, DateOnly selectedDate)
        {
            return await _repositoryContext.DentistSlots
                .Include(dl => dl.Dentist)
                .Include(dl => dl.Room)
                .Include(dl => dl.Examinations)
                .Where(dl => dl.DentistId.Equals(Guid.Parse(dentistId)) &&
                dl.TimeStart.HasValue &&
                dl.TimeStart.Value.Date == selectedDate.ToDateTime(new TimeOnly(0, 0)) &&
                dl.Room != null &&
                dl.Room.ClinicId.Equals(Guid.Parse(clinicId)) &&
                dl.Status == true)
                .ToListAsync();
        }

        public async Task<DentistSlot?> GetDentistSlotByDentistAndTimeStart(string dentistId, DateTime timeStart)
        {
            List<DentistSlot> dentistSlots = await FindByConditionAsync(dl => dl.DentistId.Equals(Guid.Parse(dentistId))
            && dl.TimeStart.Equals(timeStart));

            return dentistSlots.FirstOrDefault();
        }

        public async Task<DentistSlot?> GetDentistSlotByID(int Id)
        {
            return await _repositoryContext.DentistSlots
                .Include(dl => dl.Room)
                .Include(dl => dl.Dentist)
                .Include(dl => dl.Examinations)
                .FirstOrDefaultAsync(dl => dl.DentistSlotId == Id);
        }

        public async Task<DentistSlot?> GetDentistSlotsByRoomAndTimeStart(int roomId, DateTime timeStart)
        {
            List<DentistSlot> dentistSlots = await FindByConditionAsync(dl => dl.RoomId == roomId && dl.TimeStart.Equals(timeStart));
            return dentistSlots.FirstOrDefault();
        }
    }
}
