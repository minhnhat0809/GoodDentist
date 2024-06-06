using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repositories
{
    public interface IRoleRepo : IRepositoryBase<Role>
    {
        List<Role> GetRoles();
    }
}
