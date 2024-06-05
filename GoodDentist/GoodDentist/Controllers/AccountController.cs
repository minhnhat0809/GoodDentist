using AutoMapper;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("createUser")]
        public async  Task<ResponseCreateUserDTO> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
           ResponseCreateUserDTO responseDTO = await _accountService.createUser(createUserDTO); 
            
           return responseDTO;                       
        }

        
    }
}
