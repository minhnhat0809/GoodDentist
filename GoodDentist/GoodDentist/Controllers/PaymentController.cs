using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
using BusinessObject.DTO.PaymentDTOs;
using BusinessObject.DTO.PaymentDTOs.View;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayment([FromQuery] int pageNumber = 1, [FromQuery] int rowsPerPage = 3)
        {
            var responseDTO = await _paymentService.GetAllPayment(pageNumber, rowsPerPage);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var responseDTO = await _paymentService.GetPaymentById(id);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentAllCreateDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                return BadRequest(new ResponseDTO("Invalid data", 400, false, null));
            }

            var responseDTO = await _paymentService.CreatePayment(paymentDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePayment([FromBody] PaymentAllUpdateDTO paymentDTO)
        {
            var responseDTO = await _paymentService.UpdatePayment(paymentDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var responseDTO = await _paymentService.DeletePayment(id);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("year")]
        public async Task<ActionResult<ResponseDTO>> GetPaymentsPerYear([FromQuery] int year)
        {
            ResponseDTO responseDto = await _paymentService.GetPaymentsPerYear(year);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("date-start/date-end")]
        public async Task<ActionResult<ResponseDTO>> GetPaymentsInDateRange([FromQuery] DateTime DateStart, DateTime DateEnd)
        {
            ResponseDTO responseDto = await _paymentService.GetPaymentsInDateRange(DateStart, DateEnd);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
        
        [HttpGet("date-start/date-end/service")]
        public async Task<ActionResult<ResponseDTO>> GetPaymentsOfServicesInDateRange([FromQuery] DateTime DateStart, DateTime DateEnd)
        {
            ResponseDTO responseDto = await _paymentService.GetPaymentsOfServicesInDateRange(DateStart, DateEnd);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
