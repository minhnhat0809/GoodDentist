using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IExamProfileRepo : IRepositoryBase<ExaminationProfile>
    {
        Task<ExaminationProfile> GetExaminationProfileById(int id);
    }
}
