using BusinessObject;
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
    }
}
