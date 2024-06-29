using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repositories.Impl
{
    public class RoomRepo : RepositoryBase<Room>, IRoomRepo
    {
		public RoomRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
			
		}

		public bool CheckDuplicateRoom(CreateRoomDTO roomDTO)
		{
			Room room = _repositoryContext.Rooms.Where(t => t.RoomNumber.Equals(roomDTO.RoomNumber) && t.ClinicId==roomDTO.ClinicId).FirstOrDefault();
			if (room != null)
			{
				return true;
			}
			return false;
		}

		public async Task<List<Room>> GetAllRoom(int pageNumber, int rowsPerPage)
		{
			List<Room> roomList = await Paging(pageNumber, rowsPerPage);
			roomList.Where(s => true).ToList();
			return roomList;
		}


		public async Task<Room> GetRoomByID(int roomId)
		{
			Room room = _repositoryContext.Rooms.Where(t => t.RoomId == roomId).FirstOrDefault();
			return room;
		}
	}
}
