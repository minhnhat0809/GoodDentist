using BusinessObject;
using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ClinicServiceRepo : RepositoryBase<ClinicService>, IClinicServiceRepo
    {
        public ClinicServiceRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
