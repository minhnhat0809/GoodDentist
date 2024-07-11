using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet("/customers/denitst")]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomerOfDentist([FromQuery] string dentistId)
        {
            ResponseDTO responseDTO = await customerService.GetAllCustomerOfDentist(dentistId);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/customers")]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomer([FromQuery] string? search, int? pageNumber = 1, int? rowsPerPage = 5)
        {
            ResponseDTO responseDTO = await customerService.GetAllCustomers(search, pageNumber.Value, rowsPerPage.Value);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
