using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ClinicRepo : RepositoryBase<Clinic>, IClinicRepo
    {
        public ClinicRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Clinic?> getClinicById(string id)
        {
           return await _repositoryContext.Clinics.FirstOrDefaultAsync(c => c.ClinicId.ToString().Equals(id));
        }

        public async Task<List<Clinic>> GetClinicByUserId(Guid userId)
        {
            return await _repositoryContext.Clinics
                .Include(c => c.ClinicUsers)
                .Where(c => c.ClinicUsers.Any(x => x.UserId == userId))
                .ToListAsync();
        }
    }
}
