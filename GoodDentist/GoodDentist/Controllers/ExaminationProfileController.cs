using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
using BusinessObject.DTO.ExaminationProfileDTOs.View;

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
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/examination-profiles")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationProfiles()
        {
            ResponseDTO responseDTO = await examinationProfileService.GetAllExaminationProfiles();
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPost("/examination-profiles")]
        public async Task<ActionResult<ResponseDTO>> CreateExaminationProfile([FromBody] ExaminationProfileForExamDTO examinationProfileDTO)
        {
            ResponseDTO responseDTO = await examinationProfileService.CreateExaminationProfile(examinationProfileDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("/examination-profiles")]
        public async Task<ActionResult<ResponseDTO>> UpdateExaminationProfile([FromBody] ExaminationProfileForExamDTO examinationProfileDTO)
        {
            ResponseDTO responseDTO = await examinationProfileService.UpdateExaminationProfile(examinationProfileDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("/examination-profiles/{id}")]
        public async Task<ActionResult<ResponseDTO>> DeleteExaminationProfile(int id)
        {
            ResponseDTO responseDTO = await examinationProfileService.DeleteExaminationProfile(id);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
