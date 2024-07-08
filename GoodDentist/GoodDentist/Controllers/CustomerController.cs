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

            return responseDTO;
        }
    }
}
