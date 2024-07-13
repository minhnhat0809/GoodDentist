using BusinessObject.DTO.RoomDTOs;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRoomRepo : IRepositoryBase<Room>
	{
		bool CheckDuplicateRoom(CreateRoomDTO roomDTO);
		Task<List<Room>> GetAllRoom(int pageNumber, int rowsPerPage);
		Task<Room> GetRoomByID(int roomId);
	}
}
