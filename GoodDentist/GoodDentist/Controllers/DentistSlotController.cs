using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistSlotController : ControllerBase
    {
        private readonly IDentistSlotService _service;
        private ResponseDTO _response;

        public DentistSlotController(IDentistSlotService service, ResponseDTO response)
        {
            _service = service;
            _response = response;
        }

        [HttpGet]
        [Route("id/{id:int}")]
        public ActionResult<ResponseDTO> GetDentistSlot(int id)
        {
            try
            {
                var clinic = _service.GetDentistSlot(id);
                _response.Result = clinic;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }

            return Ok(_response);
        }

        [HttpGet]
        public ActionResult<ResponseDTO> GetDentistSlots()
        {
            try
            {
                var clinics = _service.GetDentistSlots();
                _response.Result = clinics;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }

            return Ok(_response);
        }
    }
}