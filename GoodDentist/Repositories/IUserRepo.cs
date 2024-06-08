using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepo : IRepositoryBase<User>
    {
        User? getUser(string userName);

        Task<List<User>> GetAllUsers(int pageNumber, int rowsPerPage);

        string getUserName(string Id);
    }
}
