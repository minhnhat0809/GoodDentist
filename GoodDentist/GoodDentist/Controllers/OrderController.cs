using BusinessObject.DTO;
using BusinessObject.DTO.OrderDTOs;
using BusinessObject.DTO.OrderDTOs.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
    [Route("orders")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderServices _orderservice;
        public OrderController(IOrderServices orderService)
        {
            this._orderservice = orderService;
        }

		[HttpPost("new-order")]
		public async Task<ResponseDTO> AddOrder([FromBody] OrderCreateDTO orderDTO)
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
		public async Task<ResponseDTO> UpdateOrder([FromBody] OrderUpdateDTO orderDTO)
		{
			ResponseDTO responseDTO = await _orderservice.UpdateOrder(orderDTO);

			return responseDTO;
		}

		[HttpGet("/order/detail")]
		public async Task<ActionResult<ResponseDTO>> GetOrderDetail([FromQuery] int orderId)
		{
			ResponseDTO responseDto = await _orderservice.GetOrderDetails(orderId);
			
			return StatusCode(responseDto.StatusCode, responseDto);
		}
	}
}
