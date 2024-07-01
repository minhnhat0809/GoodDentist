using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderServices _orderservice;
		private readonly IDistributedCache _distributedCache;
        public OrderController(IOrderServices orderService, IDistributedCache distributedCache)
        {
            this._orderservice = orderService;
			this._distributedCache = distributedCache;
        }

		[HttpGet("all-order")]
		public async Task<ResponseDTO> GetAllOrder([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await _orderservice.GetAllOrder(pageNumber, rowsPerPage);
			return responseDTO;
		}
	}
}
