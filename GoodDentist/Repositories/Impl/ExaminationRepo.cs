using BusinessObject;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ExaminationRepo : RepositoryBase<Examination>, IExaminationRepo
    {
        public ExaminationRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
