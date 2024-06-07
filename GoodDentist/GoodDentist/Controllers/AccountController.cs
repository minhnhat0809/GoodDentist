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
        public async  Task<ResponseCreateUserDTO> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
           ResponseCreateUserDTO responseDTO = await accountService.createUser(createUserDTO); 
            
           return responseDTO;                       
        }

        [HttpGet("all-users")]
        public async Task<ResponseDTO> GetAllUsers([FromQuery] int pageNumber, int rowsPerPage)
        {           
            ResponseDTO responseDTO = await accountService.getAllUsers(pageNumber, rowsPerPage);
            return responseDTO;
        }

        [HttpDelete("user")]
        public async Task<ResponseDTO> DeleteUser([FromQuery] string userName)
        {
            ResponseDTO responseDTO = await accountService.deleteUser(userName);
            return responseDTO;
        }

        [HttpPut("user")]
        public async Task<ResponseCreateUserDTO> UpdateUser([FromBody] CreateUserDTO createUserDTO)
        {
            ResponseCreateUserDTO responseDTO = await accountService.updateUser(createUserDTO);

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

        [HttpPut("key")]
        public async Task<string> UpdateRedisCache([FromQuery] string key, [FromQuery] string newValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "Empty key";
            }

           
            var existingValue = await distributedCache.GetStringAsync(key);

            if (existingValue == null)
            {
                return "Key not found";
            }

            
            existingValue = newValue;

           
            await distributedCache.SetStringAsync(key, existingValue);

            return "Update key successfully!";
        }

        [HttpGet("key-information")]
        public async Task<string> GetMyKey([FromQuery] string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "Empty key";
            }

            CancellationToken cancellationToken = default;

            string? existingValue = await distributedCache.GetStringAsync(key, cancellationToken);

            if (existingValue == null)
            {
                return "Key not found";
            }

            return existingValue;
        }

        [HttpPost("key-value")]
        public async Task<IActionResult> SetString([FromQuery] string key, [FromQuery] string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest("Key cannot be empty.");
            }

            if (string.IsNullOrEmpty(value))
            {
                return BadRequest("Value cannot be empty.");
            }

            try
            {
                await distributedCache.SetStringAsync(key, value);
                return Ok($"Key '{key}' with value '{value}' created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while setting key '{key}' with value '{value}': {ex.Message}");
            }
        }
    }
}
