using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;

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

        [HttpGet("/customers/dentist")]
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

        [HttpGet("/customers/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> GetCustomerById(string customerId)
        {
            ResponseDTO responseDTO = await customerService.GetCustomerById(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("/customers/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> DeleteCustomer(Guid customerId)
        {
            ResponseDTO responseDTO = await customerService.DeleteCustomer(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("/customers/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> UpdateCustomer(string customerId, [FromBody] CustomerRequestDTO customerDto)
        {
            ResponseDTO responseDTO = await customerService.UpdateCustomer(customerDto);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        [HttpPost("/customers")]
        public async Task<ActionResult<ResponseDTO>> CreateCustomer([FromBody] CustomerRequestDTO customerDto)
        {
            ResponseDTO responseDTO = await customerService.CreateCustomer(customerDto);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
