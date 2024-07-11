using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExaminationProfileController : ControllerBase
    {
        private readonly IExaminationProfileService examinationProfileService;

        public ExaminationProfileController(IExaminationProfileService examinationProfileService)
        {
            this.examinationProfileService = examinationProfileService;
        }

        [HttpGet("/examination-profiles/customer-id")]
        public async Task<ActionResult<ResponseDTO>> GetExaminationProfilesByCustomerId([FromQuery] string customerId)
        {
            ResponseDTO responseDTO = await examinationProfileService.GetExaminationProfilesByCustomerId(customerId);
            return responseDTO;
        }
    }
}
