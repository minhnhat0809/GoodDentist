using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDentistSlotService
    {
        Task<ResponseDTO> deleteDentistSlot(int slotId);

        Task<ResponseListDTO> createDentistSlot(DentistSlotDTO dentistSlotDTO);

        Task<ResponseListDTO> updateDentistSlot(DentistSlotDTO dentistSlotDTO);

        Task<ResponseDTO> getDentistSlotDetail(int slotId);

        Task<ResponseDTO> getAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage);

        Task<ResponseDTO> getAllDentistSlots(int pageNumber, int rowsPerPage);
    }
}
