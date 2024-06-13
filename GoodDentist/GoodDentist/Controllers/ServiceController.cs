using BusinessObject.DTO;
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
		private readonly IDistributedCache distributedCache;
		public ServiceController(IServiceService serviceService, IDistributedCache distributedCache)
		{
			this.serviceService = serviceService;
			this.distributedCache = distributedCache;
		}
		[HttpGet("all-services")]
		public async Task<ResponseDTO> GetAllService([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await serviceService.getAllService(pageNumber, rowsPerPage);
			return responseDTO;
		}
		[HttpPost("new-service")]
		public async Task<ResponseDTO> CreateService([FromBody] CreateServiceDTO model)
		{
			ResponseDTO responseDTO = await serviceService.createService(model);
			return responseDTO;
		}
		[HttpPut("service")]
		public async Task<ResponseDTO> UpdateService(CreateServiceDTO model)
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
