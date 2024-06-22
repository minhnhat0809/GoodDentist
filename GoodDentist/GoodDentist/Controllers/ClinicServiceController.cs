using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClinicServiceController : ControllerBase
	{
		private readonly IClinicServiceService clinicServiceService;
		private readonly IDistributedCache distributedCache;
		public ClinicServiceController(IClinicServiceService clinicServiceService, IDistributedCache distributedCache)
		{
			this.clinicServiceService = clinicServiceService;
			this.distributedCache = distributedCache;
		}
		[HttpGet]
		public async Task<ResponseDTO> GetAllClinicService([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await clinicServiceService.getAllClinicService(pageNumber, rowsPerPage);
			return responseDTO;
		}
		[HttpGet]
		public async Task<ResponseDTO> CreateClinicService([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await clinicServiceService.getAllClinicService(pageNumber, rowsPerPage);
			return responseDTO;
		}
		[HttpPost]
		public async Task<ActionResult<ResponseDTO>> CreateClinicService([FromBody] ClinicServiceDTO clinicServiceDTO)
		{
			var responseDTO = new ResponseDTO("", 200, true, null);
			try
			{
				responseDTO = await clinicServiceService.CreateClinicService(clinicServiceDTO);
			}
			catch (Exception e)
			{
				responseDTO.IsSuccess = false;
				responseDTO.Message = e.Message;
				responseDTO.StatusCode = 500;
			}

			return responseDTO;
		}
	}
}
