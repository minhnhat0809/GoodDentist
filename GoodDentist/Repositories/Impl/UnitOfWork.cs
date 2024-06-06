using BusinessObject;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodDentistDbContext _context;
        public IUserRepo userRepo { get; private set; }
        public IClinicUserRepo clinicUserRepo { get; private set; }
        public IRoleRepo roleRepo { get; private set; }
        public IClinicRepo clinicRepo { get; private set; }

        public IDistributedCache distributedCache { get; private set; }

        public UnitOfWork(GoodDentistDbContext context, IDistributedCache cache)
        {
            _context = context;
            distributedCache = cache;
            userRepo = new UserRepo(_context, distributedCache);
            clinicUserRepo = new ClinicUserRepo(_context); 
            roleRepo = new RoleRepo(_context);
            clinicRepo = new ClinicRepo(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
