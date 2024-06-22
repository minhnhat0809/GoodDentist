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
    public class ExamProfileRepo : RepositoryBase<ExaminationProfile>, IExamProfileRepo
    {
        public ExamProfileRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<ExaminationProfile> GetExaminationProfileById(int id)
        {
            return await _repositoryContext.ExaminationProfiles.FirstOrDefaultAsync(ep => ep.ExaminationProfileId == id);
        }
    }
}
