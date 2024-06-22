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
        public async Task<ResponseDTO> GetAllExaminationsOfClinic([FromQuery] int pageNumber, [FromQuery] int rowsPerPage)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;
        }

        [HttpGet("all-examinations-of-user")]
        public async Task<ResponseDTO> GetAllExaminationsOfUser([FromQuery] int pageNumber, [FromQuery] int rowsPerPage)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;
        }

        [HttpPost("new-examination")]
        public async Task<ResponseListDTO> CreateExamination([FromBody] ExaminationDTO examinationDTO)
        {
            ResponseListDTO responseDTO = await examinationService.CreateExamination(examinationDTO);

            return responseDTO;
        }

        [HttpPut("examination")]
        public async Task<ResponseListDTO> UpdateExamination([FromBody] ExaminationRequestDTO requestDto)
        {
            ResponseListDTO responseDTO = null;

            return responseDTO;
        }

        [HttpDelete("examination/{id:int}")]
        public async Task<ResponseDTO> DeleteExamination(int id)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;

        }
    }
}
