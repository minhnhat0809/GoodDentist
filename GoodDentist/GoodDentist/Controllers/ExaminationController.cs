using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService _examinationService;

        public ExaminationController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }
        [HttpGet("examination-detail/{id:int}")]
        public async Task<ResponseDTO> GetExaminationDetail(int id)
        {
            ResponseDTO responseDTO = await _examinationService.GetExamination(id);

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
        public async Task<ResponseListDTO> CreateExamination([FromBody] ExaminationRequestDTO requestDto)
        {
            ResponseListDTO responseDTO = await _examinationService.CreateExamination(requestDto);

            return responseDTO;
        }

        [HttpPut("examination")]
        public async Task<ResponseListDTO> UpdateExamination([FromBody] ExaminationRequestDTO requestDto)
        {
            ResponseListDTO responseDTO = await _examinationService.UpdateExamination(requestDto);

            return responseDTO;
        }

        [HttpDelete("examination/{id:int}")]
        public async Task<ResponseDTO> DeleteExamination(int id)
        {
            ResponseDTO responseDTO = await _examinationService.DeleteExamination(id);

            return responseDTO;

        }
    }
}
