using BusinessObject.DTO;
using BusinessObject.DTO.ExaminationDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/examinations")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService examinationService;
        public ExaminationController(IExaminationService examinationService) 
        {
            this.examinationService = examinationService;
        }

        [HttpGet("examination/detail")]
        public async Task<ActionResult<ResponseDTO>> GetExaminationDetail([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.GetExaminationById(examId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("clinic")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfClinic([FromQuery] string clinicId, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfClinic(clinicId, pageNumber, 
                rowsPerPage,sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("clinic/user")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfUser([FromQuery] string clinicId, string userId,
            DateOnly selectedDate,
            string actor , int pageNumber, int rowsPerPage,
            string? sortField = null,
            string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfUser(clinicId, userId, actor, selectedDate, pageNumber, 
                rowsPerPage, sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfExaminationProfile([FromQuery] int profileId,
           [FromQuery] int? pageNumber=1, [FromQuery] int? rowsPerPage=5,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfExaminationProfile(profileId, pageNumber.Value,
                rowsPerPage.Value, sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPost("examination")]
        public async Task<ActionResult<ResponseListDTO>> CreateExamination([FromBody] ExaminationRequestDTO examinationDTO, string mode, string? customerId)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.CreateExamination(examinationDTO, mod, mode, customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("examination")]
        public async Task<ActionResult<ResponseListDTO>> UpdateExamination([FromBody] ExaminationRequestDTO examinationDTO)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.UpdateExamination(examinationDTO, mod);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("examination")]
        public async Task<ActionResult<ResponseDTO>> DeleteExamination([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.DeleteExamination(examId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        
    }
}
