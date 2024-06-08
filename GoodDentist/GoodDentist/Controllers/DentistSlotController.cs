using BusinessObject.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistSlotController : ControllerBase
    {
        private readonly IDentistSlotService _service;

        public DentistSlotController(IDentistSlotService service)
        {
            _service = service;
            
        }

        [HttpGet]
        [Route("id/{id:int}")]
        public ActionResult<ResponseDTO> GetDentistSlot(int id)
        {
            try
            {
                var dentistSlot = _service.GetDentistSlot(id);
                return Ok(dentistSlot);
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        [HttpGet]
        public ActionResult<ResponseDTO> GetDentistSlots()
        {
            try
            {
                var dentistSlots = _service.GetDentistSlots();
                return Ok(dentistSlots);
            }
            catch (Exception e)
            {
                
            }

            return null;
        }
    }
}