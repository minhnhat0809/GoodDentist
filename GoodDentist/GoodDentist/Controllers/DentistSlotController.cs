using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ResponseDTO> GetDentistSlots([FromQuery] int pageNumber, int rowsPerPage, [FromQuery] string dentistId)
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllSlotsOfDentist(dentistId, pageNumber, rowsPerPage);

            return responseDTO;
        }

        [HttpGet("all-dentist-slots")]
        public async Task<ResponseDTO> GetAllDentistSlots([FromQuery] int pageNumber, int rowsPerPage)
        {
            ResponseDTO responseDTO = await dentistSlotService.getAllDentistSlots(pageNumber, rowsPerPage);
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
