using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _service;
        private ResponseDTO _response;

        public ClinicController(IClinicService service)
        {
            _service = service;
            _response = new ResponseDTO();
        }

        [HttpGet]
        [Route("id/{id:guid}")]
        public ActionResult<ResponseDTO> GetClinic(Guid id)
        {
            try
            {
                var clinic = _service.GetClinic(id);
                _response.result = clinic;
            }
            catch (Exception e)
            {
                _response.message = e.Message;
                _response.isSuccess = false;
            }

            return Ok(_response);
        }

        [HttpGet]
        public ActionResult<ResponseDTO> GetClinics()
        {
            try
            {
                var clinics = _service.GetClinics();
                _response.result = clinics;
            }
            catch (Exception e)
            {
                _response.message = e.Message;
                _response.isSuccess = false;
            }

            return Ok(_response);
        }
    }
}