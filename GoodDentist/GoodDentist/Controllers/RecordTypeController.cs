using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.RecordTypeDTOs;
using BusinessObject.DTO.RecordTypeDTOs.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Services;
using Services.Impl;
using System.Threading;

namespace GoodDentist.Controllers
{
    [Route("api/record-types")]
    [ApiController]
    public class RecordTypeController : ControllerBase
    {
        private readonly IRecordTypeService _recordTypeService;
        private readonly IDistributedCache _distributedCache;

        public RecordTypeController(IRecordTypeService recordTypeService, IDistributedCache distributedCache)
        {
            this._recordTypeService = recordTypeService;
            this._distributedCache = distributedCache;
        }

        [HttpGet("all-recordType")]
        public async Task<ResponseDTO> GetAllRecordType([FromQuery] int pageNumber, int rowsPerPage)
        {           
            ResponseDTO responseDTO = await _recordTypeService.GetAllRecordTyoe(pageNumber, rowsPerPage);
            return responseDTO;
        }

        [HttpPost("new-recordType")]
        public async Task<ResponseDTO> AddRecordType([FromBody] RecordTypeCreateDTO recordTypeDTO)
        {
            ResponseDTO responseDTO = await _recordTypeService.AddRecordType(recordTypeDTO);

            return responseDTO;
        }

        [HttpDelete("recordType")]

        public async Task<ResponseDTO> DeleteRecordType([FromQuery] int recordTypeId)
        {
            ResponseDTO responseDTO = await _recordTypeService.DeleteRecordType(recordTypeId);

            return responseDTO;
        }

        [HttpGet("searchRecordType")]

        public async Task<ResponseDTO> SearchRecordType([FromQuery] string searchValue)
        {
            ResponseDTO responseDTO = await _recordTypeService.SearchRecordType(searchValue);

            return responseDTO;
        }

        [HttpPut("updateRecordType")]
        public async Task<ResponseDTO> UpdateRecordType([FromBody] RecordTypeDTO recordTypeDTO)
        {
            ResponseDTO responseDTO = await _recordTypeService.UpdateRecordType(recordTypeDTO);

            return responseDTO;
        }       
    }
}
