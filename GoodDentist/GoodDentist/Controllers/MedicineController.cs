using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.MedicineDTOs;
using BusinessObject.DTO.MedicineDTOs.View;
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
    [Route("api/medicines")]
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
        public async Task<ResponseDTO> GetAllMedicine([FromQuery] string? filterField = null, 
            string? filterValue = null,
            string? sortField = null,
            string? sortValue = "asc",
            string? search = null,
            int? pageNumber = 1, int? rowsPerPage = 5)
        {           
            ResponseDTO responseDTO = await _medicineService.GetAllMedicine(filterField
            , filterValue, sortField, sortValue, search, pageNumber, rowsPerPage);
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
