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
            
        }

        [HttpPost]
        public async Task<ResponseLoginDTO> Login([FromBody]LoginDTO loginDto)
        {
            _ResponseDto = new ResponseDTO("Login Successfully", 200, true, null);
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
        
        [HttpGet("users")]
        public async Task<ResponseDTO> GetUsers()
        {
            _ResponseDto = new ResponseDTO("Get User Successfully", 200, true, null);
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

        [HttpPost("reset-password")]
        public async Task<ResponseLoginDTO> ResetPassword([FromBody] LoginDTO loginDto)
        {
            _Response = new ResponseLoginDTO();
            try
            {
                _Response = await _authService.ResetPassword(loginDto);
              
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
            }

            return _Response;
        }

        [HttpGet("user/{emailOrPhone}")]
        public async Task<ResponseDTO> GetAccountByEmailOrPhone( string emailOrPhone)
        {
            _ResponseDto = new ResponseDTO("Get Account Successfully", 200, true, null);
            try
            {
                _ResponseDto = await _authService.GetAccountByEmailOrPhone(emailOrPhone);
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

