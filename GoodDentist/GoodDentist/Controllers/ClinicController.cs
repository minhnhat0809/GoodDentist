using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _service;
        public ClinicController(IClinicService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("id/{id:guid}")]
        public async Task<IActionResult> GetClinic(Guid id)
        {
            try
            {
                var clinic = await _service.GetClinic(id);
                return Ok(clinic);
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetClinics()
        {
            try
            {
                var clinics = await _service.GetClinics();
                return Ok(clinics);
            }
            catch (Exception e)
            {
                
            }

            return null;
        }
    }
}