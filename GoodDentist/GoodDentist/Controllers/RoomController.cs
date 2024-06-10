using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using Services.Impl;

namespace GoodDentist.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoomController : ControllerBase
	{
		private readonly IRoomService roomService;
		private readonly IDistributedCache distributedCache;
		public RoomController(IRoomService roomService, IDistributedCache distributedCache)
		{
			this.roomService = roomService;
			this.distributedCache = distributedCache;
		}
		[HttpGet("all-rooms")]
		public async Task<ResponseDTO> GetAllService([FromQuery] int pageNumber, int rowsPerPage)
		{
			ResponseDTO responseDTO = await roomService.getAllRoom(pageNumber, rowsPerPage);
			return responseDTO;
		}
		[HttpPost("new-room")]
		public async Task<ResponseDTO> CreateRoom([FromBody] CreateRoomDTO model)
		{
			ResponseDTO responseDTO = await roomService.createRoom(model);
			return responseDTO;
		}
		[HttpPut("room")]
		public async Task<ResponseDTO> UpdateService([FromBody] CreateRoomDTO model)
		{
			ResponseDTO responseDTO = await roomService.updateRoom(model);
			return responseDTO;
		}
		[HttpDelete("room")]
		public async Task<ResponseDTO> DeleteService([FromBody] int roomId)
		{
			ResponseDTO responseDTO = await roomService.deleteRoom(roomId);
			return responseDTO;
		}
	}
}
