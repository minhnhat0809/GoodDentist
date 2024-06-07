using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistSlotController : ControllerBase
    {
        [HttpGet("dentist-slot-detail")]
        public async Task<ResponseDTO> GetDentistSlotDetail()
        {
            return null;
        }

        [HttpGet("dentist-slots")]
        public async Task<ResponseDTO> GetDentistSlots([FromQuery] int pageNumber, int rowsPerPage)
        {
            return null;
        }

        [HttpGet("all-dentist-slots")]
        public async Task<ResponseDTO> GetAllDentistSlots([FromQuery] int pageNumber, int rowsPerPage)
        {
            return null;
        }


        [HttpPost("new-dentist-slot")]
        public async Task<ResponseDTO> CreateDentistSlot()
        {
            return null;
        }

        [HttpPut("dentist-slot")]
        public async Task<ResponseDTO> UpdateDentistSlot()
        {
            return null;
        }

        [HttpDelete("dentist-slot")]
        public async Task<ResponseDTO> DeleteDentistSlot()
        {
            return null;
        }


    }
}
