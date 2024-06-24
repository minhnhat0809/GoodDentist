using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Repositories.Impl;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        public ResponseLoginDTO _Response;
        public ResponseDTO _ResponseDto;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
            _Response = new ResponseLoginDTO();
            _ResponseDto = new ResponseDTO("", 200, true, null);
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
        [HttpGet]
        public async Task<ResponseDTO> GetUser()
        {
            try
            {
                // How to take access token and claim the username or user id from it 
                var accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(accessToken);
                var userIdString = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                var userId = Guid.Parse(userIdString);
                _ResponseDto = await _authService.GetUserAsync(userId);
                
            }
            catch (Exception e)
            {
                _ResponseDto.IsSuccess = false;
                _ResponseDto.Message = e.Message;
            }

            return _ResponseDto;
        }
    }
}

