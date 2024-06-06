using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepo : IRepositoryBase<User>
    {
        void CreateUser(User user);

        bool checkExistUser(string userName);

        Task<List<User>> GetAllUsers();
    }
}
