using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/dentist-slots")]
    [ApiController]
    public class DentistSlotController : ControllerBase
    {
        private readonly IDentistSlotService dentistSlotService;

        public DentistSlotController(IDentistSlotService dentistSlotService)
        {
            this.dentistSlotService = dentistSlotService;
        }

        [HttpGet("dentist-slot-detail")]
        public async Task<ResponseDTO> GetDentistSlotDetail([FromQuery] int slotId)
        {
            ResponseDTO responseDTO = await dentistSlotService.getDentistSlotDetail(slotId);
            return responseDTO;
        }

        [HttpGet("dentist-dentist-slots")]
        public async Task<ResponseDTO> GetAllSlotsOfDentist([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string dentistId,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllSlotsOfDentist(dentistId, pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet("clinic-dentist-slots")]
        public async Task<ResponseDTO> GetAllSlotsOfClinic([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string clinicId,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllSlotsOfClinic(clinicId, pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet("all-dentist-slots")]
        public async Task<ResponseDTO> GetAllDentistSlots([FromQuery] int pageNumber,
            [FromQuery] int rowsPerPage,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllDentistSlots(pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
            return responseDTO;
        }

        [HttpPost("new-dentist-slot")]
        public async Task<ResponseListDTO> CreateDentistSlot([FromBody] DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = await dentistSlotService.createDentistSlot(dentistSlotDTO);
            return responseDTO;
        }

        [HttpPut("dentist-slot")]
        public async Task<ResponseListDTO> UpdateDentistSlot([FromBody] DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = await dentistSlotService.updateDentistSlot(dentistSlotDTO);
            return responseDTO;
        }

        [HttpDelete("dentist-slot")]
        public async Task<ResponseDTO> DeleteDentistSlot([FromQuery] int slotId)
        {
            ResponseDTO responseDTO = await dentistSlotService.deleteDentistSlot(slotId);
            return responseDTO;
        }


    }
}
