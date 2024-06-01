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
    }
}
