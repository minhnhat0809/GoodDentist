using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
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

        [HttpGet("examination-detail")]
        public async Task<ResponseDTO> GetExaminationDetail([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.GetExaminationById(examId);
            return responseDTO;
        }

        [HttpGet("all-examinations-of-clinic")]
        public async Task<ResponseDTO> GetAllExaminationsOfClinic([FromQuery] string clinicId, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfClinic(clinicId, pageNumber, 
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet("/clinic/user")]
        public async Task<ResponseDTO> GetAllExaminationsOfUser([FromQuery] string clinicId, [FromQuery] string userId, [FromQuery] string actor , [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfUser(clinicId, userId, actor, pageNumber, 
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return responseDTO;
        }

        [HttpGet("/clinic/profile")]
        public async Task<ResponseDTO> GetAllExaminationsOfExaminationProfile([FromQuery] int profileId, [FromQuery] string userId, [FromQuery] string actor, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfExaminationProfile(profileId, pageNumber,
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return responseDTO;
        }

        [HttpPost("/new-examination")]
        public async Task<ResponseListDTO> CreateExamination([FromBody] ExaminationDTO examinationDTO)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.CreateExamination(examinationDTO, mod);

            return responseDTO;
        }

        [HttpPut("examination")]
        public async Task<ResponseListDTO> UpdateExamination([FromBody] ExaminationDTO examinationDTO)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.UpdateExamination(examinationDTO, mod);
            return responseDTO;
        }

        [HttpDelete("examination")]
        public async Task<ResponseDTO> DeleteExamination([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.DeleteExamination(examId);
            return responseDTO;
        }
    }
}
