using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("/dentist-slots")]
    [ApiController]
    public class DentistSlotController : ControllerBase
    {
        private readonly IDentistSlotService dentistSlotService;

        public DentistSlotController(IDentistSlotService dentistSlotService)
        {
            this.dentistSlotService = dentistSlotService;
        }

        [HttpGet("/dentist-slot/detail")]
        public async Task<ResponseDTO> GetDentistSlotDetail([FromQuery] int slotId)
        {
            ResponseDTO responseDTO = await dentistSlotService.getDentistSlotDetail(slotId);
            return responseDTO;
        }

        [HttpGet("/dentist")]
        public async Task<ResponseDTO> GetAllSlotsOfDentist([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string dentistId,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllSlotsOfDentist(dentistId, pageNumber, rowsPerPage, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet("/clinic")]
        public async Task<ResponseDTO> GetAllSlotsOfClinic([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string clinicId,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllSlotsOfClinic(clinicId, pageNumber, rowsPerPage, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllDentistSlots([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllDentistSlots(pageNumber, rowsPerPage, sortField, sortOrder);
            return responseDTO;
        }

        [HttpPost("/dentist-slot")]
        public async Task<ResponseListDTO> CreateDentistSlot([FromBody] DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = await dentistSlotService.createDentistSlot(dentistSlotDTO);
            return responseDTO;
        }

        [HttpPut("/dentist-slot")]
        public async Task<ResponseListDTO> UpdateDentistSlot([FromBody] DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = await dentistSlotService.updateDentistSlot(dentistSlotDTO);
            return responseDTO;
        }

        [HttpDelete("/dentist-slot")]
        public async Task<ResponseDTO> DeleteDentistSlot([FromQuery] int slotId)
        {
            ResponseDTO responseDTO = await dentistSlotService.deleteDentistSlot(slotId);
            return responseDTO;
        }

        [HttpGet("/dentist/time-start/time-end")]
        public async Task<ActionResult<ResponseDTO>> GetAllDentistSlotsByDentistAndTimeStart([FromQuery] string clinicId,
        DateTime timeStart, DateTime timeEnd)
        {
            ResponseDTO responseDTO = await dentistSlotService.GetAllSlotsOfDentistByTimeStart(clinicId, timeStart, timeEnd);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/dentist/date")]
        public async Task<ActionResult<ResponseDTO>> GetAllDentistSlotsByDentistAndDate([FromQuery] string clinicId, string dentistId, DateOnly selectedDate)
        {
            ResponseDTO responseDTO = await dentistSlotService.GetAllDentistSlotsByDentistAndDate(clinicId, dentistId, selectedDate);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
