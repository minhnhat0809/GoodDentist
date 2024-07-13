using BusinessObject.DTO;
using BusinessObject.DTO.DentistSlotDTOs;
using Microsoft.Data.SqlClient;
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

        Task<ResponseListDTO> createDentistSlot(CreateDentistSlotDTO dentistSlotDTO);

        Task<ResponseListDTO> updateDentistSlot(CreateDentistSlotDTO dentistSlotDTO);

        Task<ResponseDTO> getDentistSlotDetail(int slotId);

        Task<ResponseDTO> getAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage,
        string sortField, string sortOrder);

        Task<ResponseDTO> getAllDentistSlots(int pageNumber, int rowsPerPage,
        string sortField, string sortOrder);

        Task<ResponseDTO> getAllSlotsOfClinic(string clinicId, int pageNumber, int rowsPerPage,
        string sortField, string sortOrder);

        Task<ResponseDTO> GetAllSlotsOfDentistByTimeStart(string clinicId, DateTime timeStart, DateTime timeEnd);

        Task<ResponseDTO> GetAllDentistSlotsByDentistAndDate(string clinicId, string dentistId, DateOnly selectedDate);
    }
}
