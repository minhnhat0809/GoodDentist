using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodDentist.Controllers
{
    [Route("api/examinations")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        [HttpGet("examination-detail")]
        public async Task<ResponseDTO> GetExaminationDetail([FromQuery] int examId)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;
        }

        [HttpGet("all-examinations-of-clinic")]
        public async Task<ResponseDTO> GetAllExaminationsOfClinic([FromQuery] int pageNumber, [FromQuery] int rowsPerPage)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;
        }

        [HttpGet("all-examinations-of-user")]
        public async Task<ResponseDTO> GetAlllExaminationsOfUser([FromQuery] int pageNumber, [FromQuery] int rowsPerPage)
        {
            ResponseDTO responseDTO = null;

            return responseDTO;
        }

        [HttpPost("new-examination")]
        public async Task<ResponseListDTO> CreateExamination()
        {
            ResponseListDTO responseDTO = null;

            return responseDTO;
        }

        [HttpPut("examination")]
        public async Task<ResponseListDTO> UpdateExamination()
        {
            ResponseListDTO responseDTO = null;

            return responseDTO;
        }

        [HttpDelete("examination")]
        public async Task<ResponseDTO> DeleteExamination()
        {
            ResponseDTO responseDTO = null;

            return responseDTO;

        }
    }
}
