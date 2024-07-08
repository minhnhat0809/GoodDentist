using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllCustomerOfDentist([FromQuery] string dentistId)
        {
            ResponseDTO responseDTO = await customerService.GetAllCustomerOfDentist(dentistId);

            return responseDTO;
        }
    }
}
