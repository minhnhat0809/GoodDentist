using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralService generalService;

        public GeneralController(IGeneralService generalService)
        {
            this.generalService = generalService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetTotal ([FromQuery] string type)
        {
            ResponseDTO responseDTO = await generalService.GetTotal(type);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
