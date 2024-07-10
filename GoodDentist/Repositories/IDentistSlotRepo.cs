using BusinessObject.DTO;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IDentistSlotRepo : IRepositoryBase<DentistSlot>
    {
        Task<DentistSlot?> GetDentistSlotByID(int Id);

        Task<List<DentistSlot>?> GetAllDentistSlots(int pageNumber, int rowsPerPage);

        Task<List<DentistSlot>?> GetAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage);

        Task<DentistSlot?> GetDentistSlotsByRoomAndTimeStart(int roomId, DateTime timeStart);

        Task<DentistSlot?> GetDentistSlotByDentistAndTimeStart(string dentistId, DateTime timeStart);

        Task<List<DentistSlot>?> GetAllSlotsOfClinic(string clinicId, int pageNumber, int rowsPerPage);

        Task<List<DentistSlot>?> GetAllDentistSlotsByDentistAndTimeStart(string clinicId, DateTime timeStart, DateTime timeEnd);
    }
}
