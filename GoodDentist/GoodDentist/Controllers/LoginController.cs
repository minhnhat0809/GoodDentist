using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Repositories.Impl;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        public ResponseLoginDTO _Response;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
            _Response = new ResponseLoginDTO();
        }

        [HttpPost]
        public async Task<ResponseLoginDTO> Login([FromBody]LoginDTO loginDto)
        {
            try
            {
                _Response = await _authService.Authenticate(loginDto);
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
            }

            return _Response;
        }
    }
}

