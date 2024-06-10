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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService accountService;
        private readonly IDistributedCache distributedCache;

        public AccountController(IUserService accountService, IDistributedCache distributedCache)
        {
            this.accountService = accountService;
            this.distributedCache = distributedCache;
        }


        [HttpPost("new-user")]
        public async  Task<ResponseListDTO> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = await accountService.createUser(createUserDTO); 
            
           return responseDTO;                       
        }

        [HttpGet("all-users")]
        public async Task<ResponseDTO> GetAllUsers([FromQuery] int pageNumber, int rowsPerPage,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {           
            ResponseDTO responseDTO = await accountService.getAllUsers(pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
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
            ResponseDTO responseDTO = await accountService.getAllUsersByClinic(clinicId, pageNumber, rowsPerPage, filterField, filterValue, sortField, sortOrder);
            return responseDTO;
        }

        [HttpDelete("user")]
        public async Task<ResponseDTO> DeleteUser([FromQuery] string userName)
        {
            ResponseDTO responseDTO = await accountService.deleteUser(userName);
            return responseDTO;
        }

        [HttpPut("user")]
        public async Task<ResponseListDTO> UpdateUser([FromBody] CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = await accountService.updateUser(createUserDTO);

            return responseDTO;
        }









        [HttpDelete("key")]
        public async Task<string> deleteRedisCache([FromQuery] string key)
        {
            if (key.IsNullOrEmpty())
            {
                return "Empty key";
            }
            else
            {
                CancellationToken cancellationToken = default;
                string? checkCache = await distributedCache.GetStringAsync(key, cancellationToken);

                if (checkCache.IsNullOrEmpty())
                {
                    return "No value with this key";
                }

               await distributedCache.RemoveAsync(key);
                return "Remove key successfully!";
            }           
        }
    }
}
