using BusinessObject;
using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRoleRepo : IRepositoryBase<Role>
    {
        Task<Role?> GetRole(int id);
        Role? GetRoleById(int id);
    }
}
