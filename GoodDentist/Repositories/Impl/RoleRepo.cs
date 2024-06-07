using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class RoleRepo : RepositoryBase<Role>, IRoleRepo
    {
        public RoleRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Role?> GetRole(int id)
        {
            List<Role> roleList = await FindAllAsync();
            Role? role = roleList.FirstOrDefault(r => r.RoleId == id);
            return role;
        }
    }
}
