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
    public class ExaminationRepo : RepositoryBase<Examination>, IExaminationRepo
    {
        public ExaminationRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Examination?> GetExaminationById(int examId)
        {
            Examination? examination = await _repositoryContext.Examinations.FirstOrDefaultAsync(ex => ex.ExaminationId == examId);

            return examination;
        }
    }
}
