using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;

namespace GoodDentist.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PrescriptionController : ControllerBase
	{
		private readonly IPrescriptionService _prescriptionService;
		private readonly IDistributedCache _distributedCache;
        public PrescriptionController(IPrescriptionService prescriptionService, IDistributedCache distributedCache)
        {
            this._prescriptionService = prescriptionService;
			this._distributedCache = distributedCache;
        }
		/*[HttpPost("new-order")]
		public async Task<ResponseDTO> AddOrder([FromBody] OrderCreateDTO orderDTO)
		{
			ResponseDTO responseDTO = await _orderservice.AddOrder(orderDTO);

			return responseDTO;
		}*/

		[HttpGet("all-prescription")]
		public async Task<ResponseDTO> GetAllPrescription([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await _prescriptionService.GetAllPrescription(pageNumber, rowsPerPage);
			return responseDTO;
		}
	}
}
