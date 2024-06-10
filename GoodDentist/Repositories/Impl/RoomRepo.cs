using BusinessObject;
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
		private readonly IDistributedCache distributedCache;
		public RoomRepo(GoodDentistDbContext repositoryContext, IDistributedCache distributedCache) : base(repositoryContext)
        {
			this.distributedCache = distributedCache;
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
