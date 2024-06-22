using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{

    [Route("api/clinics")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<ResponseDTO>> CreateClinic([FromBody] ClinicRequestDTO requestDto)
        {
            _responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                var createdClinic = await _service.CreateClinic(requestDto);
                _responseDto.Result = createdClinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }

        
        [HttpPut]
        public async Task<ActionResult<ResponseDTO>> UpdateClinic([FromBody] ClinicRequestDTO requestDto)
        {
            _responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                var updatedClinic = await _service.UpdateClinic(requestDto);
                _responseDto.Result = updatedClinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseDTO>> DeleteClinic(Guid id)
        {
            _responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                var deletedClinic = await _service.DeleteClinic(id);
                if (deletedClinic == null)
                {
                    return NotFound(); // Or handle not found scenario
                }
                _responseDto.Result = deletedClinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
    }
}