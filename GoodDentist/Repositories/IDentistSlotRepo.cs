using BusinessObject;
using BusinessObject.DTO;
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
    }
}
