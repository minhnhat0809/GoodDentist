using BusinessObject.DTO;
using BusinessObject.DTO.ServiceDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/services")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly IServiceService serviceService;
		public ServiceController(IServiceService serviceService)
		{
			this.serviceService = serviceService;
		}
		[HttpGet("all-services")]
		public async Task<ActionResult<ResponseDTO>> GetAllServices(
			[FromQuery] int pageNumber = 1, int rowsPerPage = 5,
			[FromQuery] string? filterField = null,
			[FromQuery] string? filterValue = null,
			[FromQuery] string? sortField = null,
			[FromQuery] string? sortOrder = "asc")
		{
			ResponseDTO responseDTO = await serviceService.GetAllServices(pageNumber,rowsPerPage,filterField,filterValue,sortField,sortOrder);
            
			return StatusCode(responseDTO.StatusCode, responseDTO);
		}

		[HttpGet("clinic")]
		public async Task<ActionResult<ResponseDTO>> GetAllServiceByClinic([FromQuery] string clinicId,
			string? filterField,
			string? filterValue,
			int? pageNumber = 1,
			int? rowsPerPage = 5)
		{
			ResponseDTO responseDto = await serviceService.GetAllServicesByClinic(clinicId, filterField, filterValue, pageNumber, rowsPerPage);
			return StatusCode(responseDto.StatusCode, responseDto);
		}
		
		[HttpPost("new-service")]
		public async Task<ResponseDTO> CreateService([FromBody] CreateServiceDTO model)
		{
			ResponseDTO responseDTO = await serviceService.createService(model);
			return responseDTO;
		}
		[HttpPut("service")]
		public async Task<ResponseDTO> UpdateService([FromBody] CreateServiceDTO model)
		{
			ResponseDTO responseDTO = await serviceService.updateService(model);
			return responseDTO;
		}
		[HttpDelete("service")]
		public async Task<ResponseDTO> DeleteService([FromBody] int serviceID)
		{
			ResponseDTO responseDTO = await serviceService.deleteService(serviceID);
			return responseDTO;
		}
	}
}
