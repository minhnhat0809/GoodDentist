using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;

namespace Services;

public interface IRoomService
{
	Task<ResponseDTO> createRoom(CreateRoomDTO model);
	Task<ResponseDTO> deleteRoom(int roomId);
	Task<ResponseDTO> getAllRoom(int pageNumber, int rowsPerPage);
	Task<ResponseDTO> updateRoom(CreateRoomDTO model);
	ResponseDTO ValidateRoom(CreateRoomDTO serviceDTO);
}
