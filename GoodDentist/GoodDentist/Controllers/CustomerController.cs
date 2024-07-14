using BusinessObject.DTO;
using BusinessObject.DTO.CustomerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet("denitst")]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomerOfDentist([FromQuery] string dentistId, string? search)
        {
            ResponseDTO responseDTO = await customerService.GetAllCustomerOfDentist(dentistId, search);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomer(
            [FromQuery] int pageNumber = 1, int rowsPerPage = 5,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await customerService.GetCustomers(pageNumber,rowsPerPage,filterField,filterValue,sortField,sortOrder);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("clinic")]
        public async Task<ActionResult<ResponseDTO>> GetCustomersByClinic([FromQuery] string clinicId,
           [FromQuery] int pageNumber, int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await customerService.GetCustomersByClinic(clinicId, pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> GetCustomerById(string customerId)
        {
            ResponseDTO responseDTO = await customerService.GetCustomerById(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("customer/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> DeleteCustomer(Guid customerId)
        {
            ResponseDTO responseDTO = await customerService.DeleteCustomer(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        [HttpPut("customer")]
        public async Task<ActionResult<ResponseDTO>> UpdateCustomer([FromForm]CustomerUpdateRequestDTO customerRequestDto)
        {
            ResponseListDTO responseDTO = await customerService.UpdateCustomer(customerRequestDto);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        [HttpPost("customer")]
        public async Task<ActionResult<ResponseDTO>> CreateCustomer([FromForm] CustomerRequestDTO customerDto)
        {
            ResponseDTO responseDTO = await customerService.CreateCustomer(customerDto);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        [HttpDelete("customer/avatar/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> DeleteFile(Guid customerId)
        {
            ResponseDTO responseDTO = await customerService.DeleteFileAndReference(customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("customer/avatar/{customerId}")]
        public async Task<ActionResult<ResponseDTO>> UpdateFile(Guid customerId, IFormFile uploadFile)
        {
            ResponseDTO responseDTO = await customerService.UploadFile(uploadFile, customerId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        
        
    }
}
