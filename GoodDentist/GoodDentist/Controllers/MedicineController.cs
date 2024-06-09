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
        [HttpGet("medicine")]
        public async Task<ResponseDTO> SearchMedicine([FromQuery] string searchValue)
        {
            ResponseDTO responseDTO = await _medicineService.SearchMedicine(searchValue);
            return responseDTO;
        }
    }
}
