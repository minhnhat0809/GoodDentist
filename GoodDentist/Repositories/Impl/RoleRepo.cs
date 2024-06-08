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

        public async Task<Role?> GetRole(int id)
        {
            List<Role> roleList = await _context.Roles.ToListAsync();
            Role? role = roleList.FirstOrDefault(r => r.RoleId == id);
            return role;
        }

        public Role? GetRoleById(int id)
        {
            return _context.Roles.Find(id);
        }
    }
}
