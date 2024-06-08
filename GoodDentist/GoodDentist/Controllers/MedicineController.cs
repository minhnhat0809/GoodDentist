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
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IDistributedCache _distributedCache;

        public MedicineController(IMedicineService medicineService, IDistributedCache distributedCache)
        {
            this._medicineService = medicineService;
            this._distributedCache = distributedCache;
        }

        [HttpPost("new-medicine")]
        public async  Task<ResponseDTO> AddMedicine([FromBody] MedicineDTO medicineDTO)
        {
           ResponseDTO responseDTO = await _medicineService.AddMedicine(medicineDTO); 
            
           return responseDTO;                       
        }

        [HttpGet("all-medicine")]
        public async Task<ResponseDTO> GetAllMedicine([FromQuery] int pageNumber, int rowsPerPage)
        {           
            ResponseDTO responseDTO = await _medicineService.GetAllMedicine(pageNumber, rowsPerPage);
            return responseDTO;
        }

        [HttpDelete("medicine")]
        public async Task<ResponseDTO> DeleteMedicine([FromQuery] int medicineId)
        {
            ResponseDTO responseDTO = await _medicineService.DeleteMedicine(medicineId);
            return responseDTO;
        }

        [HttpPut("medicine")]
        public async Task<ResponseDTO> UpdateMedicine([FromBody] MedicineUpdateDTO medicineDTO)
        {
            ResponseDTO responseDTO = await _medicineService.UpdateMedicine(medicineDTO);

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
                string? checkCache = await _distributedCache.GetStringAsync(key, cancellationToken);

                if (checkCache.IsNullOrEmpty())
                {
                    return "No value with this key";
                }

               await _distributedCache.RemoveAsync(key);
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

           
            var existingValue = await _distributedCache.GetStringAsync(key);

            if (existingValue == null)
            {
                return "Key not found";
            }

            
            existingValue = newValue;

           
            await _distributedCache.SetStringAsync(key, existingValue);

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

            string? existingValue = await _distributedCache.GetStringAsync(key, cancellationToken);

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
                await _distributedCache.SetStringAsync(key, value);
                return Ok($"Key '{key}' with value '{value}' created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while setting key '{key}' with value '{value}': {ex.Message}");
            }
        }
    }
}
