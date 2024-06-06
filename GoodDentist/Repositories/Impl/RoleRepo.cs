using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl
{
    public class RoleRepo : RepositoryBase<Role>, IRoleRepo
    {
        private readonly GoodDentistDbContext _context;

        public RoleRepo(GoodDentistDbContext context) : base(new GoodDentistDbContext())
        {
            _context = context;
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
