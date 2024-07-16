using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ClinicDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{

    [Route("api/clinics")]
    [ApiController]
    
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _service;
        public ResponseDTO _responseDto;
        public ClinicController(IClinicService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("id/{id:guid}")]
        public async Task<ResponseDTO> GetClinic(Guid id)
        {
            _responseDto = new ResponseDTO("", 200,true,null);
            try
            {
                var clinic = await _service.GetClinic(id);
                _responseDto.Result = clinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
        [HttpGet]
        [Route("user/{id:guid}")]
        public async Task<ResponseDTO> GetClinicByUserId(Guid id)
        {
            _responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                var clinic = await _service.GetClinicByUserId(id);
                _responseDto.Result = clinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
        [HttpGet("paging")]
        public async Task<ActionResult<ResponseDTO>> GetAllClinics(
            [FromQuery] int pageNumber = 1, int rowsPerPage = 5,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await _service.GetAllClinics(pageNumber,rowsPerPage,filterField,filterValue,sortField,sortOrder);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
        [HttpGet]
        public async Task<ResponseDTO> GetClinics(
            [FromQuery] string? filterOn ,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, 
            [FromQuery] bool isAscending)

        {
            _responseDto = new ResponseDTO("", 200,true,null);
            try
            {
                var clinics = await _service.GetClinics(filterOn, filterQuery,sortBy,isAscending);
                _responseDto.Result = clinics;

            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> CreateClinic([FromBody] ClinicCreateDTO requestDto)
        {
            _responseDto = await _service.CreateClinic(requestDto);
            return _responseDto;
        }

        
        [HttpPut]
        public async Task<ActionResult<ResponseDTO>> UpdateClinic([FromBody] ClinicUpdateDTO requestDto)
        {
            _responseDto = await _service.UpdateClinic(requestDto);
            return _responseDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteClinic(Guid id)
        {
            
            _responseDto = await _service.DeleteClinic(id);
            return _responseDto;
        }
    }
}