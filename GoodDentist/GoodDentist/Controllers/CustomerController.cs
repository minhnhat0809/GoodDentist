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
        public async Task<ActionResult<ResponseDTO>> GetAllCustomerOfDentist([FromQuery] string dentistId, string? search)
        {
            ResponseDTO responseDTO = await customerService.GetAllCustomerOfDentist(dentistId, search);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/customers")]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomer(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1, int rowsPerPage = 5,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await customerService.GetCustomers(search, pageNumber,rowsPerPage,filterField,filterValue,sortField,sortOrder);
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
        public async Task<ActionResult<ResponseDTO>> UpdateCustomer([FromBody] CustomerRequestDTO customerDto)
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
        
        [HttpDelete("/customers/avatar/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> DeleteFile(Guid customerId)
        {
            ResponseDTO responseDTO = await customerService.DeleteFileAndReference(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("/customers/avatar/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> UpdateFile(Guid customerId, IFormFile uploadFile)
        {
            ResponseDTO responseDTO = await customerService.UploadFile(uploadFile, customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        
    }
}
