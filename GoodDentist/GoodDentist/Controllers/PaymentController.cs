using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllPayment([FromQuery] int pageNumber, [FromQuery] int rowsPerPage)
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
        public async Task<IActionResult> CreatePayment([FromBody] PaymentAllDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                return BadRequest(new ResponseDTO("Invalid data", 400, false, null));
            }

            var responseDTO = await _paymentService.CreatePayment(paymentDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentAllDTO paymentDTO)
        {
            if (paymentDTO == null || id != paymentDTO.PaymentAllId)
            {
                return BadRequest(new ResponseDTO("Invalid data", 400, false, null));
            }

            var responseDTO = await _paymentService.UpdatePayment(paymentDTO);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var responseDTO = await _paymentService.DeletePayment(id);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
