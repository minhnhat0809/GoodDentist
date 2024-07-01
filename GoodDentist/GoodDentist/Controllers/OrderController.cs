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

		[HttpPost("new-order")]
		public async Task<ResponseDTO> AddOrder([FromBody] OrderDTO orderDTO)
		{
			ResponseDTO responseDTO = await _orderservice.AddOrder(orderDTO);

			return responseDTO;
		}

		[HttpGet("all-order")]
		public async Task<ResponseDTO> GetAllOrder([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await _orderservice.GetAllOrder(pageNumber, rowsPerPage);
			return responseDTO;
		}

		[HttpGet("search-order")]
		public async Task<ResponseDTO> SearchOrder([FromQuery] string searchValue)
		{
			ResponseDTO responseDTO = await _orderservice.SearchOrder(searchValue);
			return responseDTO;
		}

		[HttpDelete("delete-order")]
		public async Task<ResponseDTO> DeleteOrder([FromQuery] int orderId)
		{
			ResponseDTO responseDTO = await _orderservice.DeleteOrder(orderId);
			return responseDTO;
		}

		[HttpPut("update-order")]
		public async Task<ResponseDTO> UpdateOrder([FromBody] OrderDTO orderDTO)
		{
			ResponseDTO responseDTO = await _orderservice.UpdateOrder(orderDTO);

			return responseDTO;
		}
	}
}
