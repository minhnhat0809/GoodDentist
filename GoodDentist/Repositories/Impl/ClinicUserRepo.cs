using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ClinicUserRepo : RepositoryBase<ClinicUser>, IClinicUserRepo
    {
        public ClinicUserRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }



    }
}
