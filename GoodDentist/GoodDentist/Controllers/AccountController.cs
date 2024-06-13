using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Services;
using System.Threading;

namespace GoodDentist.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IDistributedCache distributedCache;

        public AccountController(IUserService accountService, IDistributedCache distributedCache)
        {
            this.userService = accountService;
            this.distributedCache = distributedCache;
        }


        [HttpPost("new-user")]
        public async  Task<ResponseListDTO> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = await userService.createUser(createUserDTO); 
            
           return responseDTO;                       
        }

        [HttpGet("all-users")]
        public async Task<ResponseDTO> GetAllUsers([FromQuery] int pageNumber, int rowsPerPage,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {           
            ResponseDTO responseDTO = await userService.getAllUsers(pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
            return responseDTO;
        }

        [HttpGet("all-users-by-clinic")]
        public async Task<ResponseDTO> GetAllUsersByClinic([FromQuery] string clinicId,
           [FromQuery] int pageNumber, int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await userService.getAllUsersByClinic(clinicId, pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
            return responseDTO;
        }

        [HttpDelete("user")]
        public async Task<ResponseDTO> DeleteUser([FromQuery] string userName)
        {
            ResponseDTO responseDTO = await userService.deleteUser(userName);
            return responseDTO;
        }

        [HttpPut("user")]
        public async Task<ResponseListDTO> UpdateUser([FromBody] CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = await userService.updateUser(createUserDTO);

            return responseDTO;
        }









        [HttpDelete("key")]
        public async Task<string> deleteRedisCache([FromQuery] string key)
        {
            string result = await userService.deleteCache(key);
            return result;
                
        }
    }
}
