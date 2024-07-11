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
        Task<List<ExaminationProfile>> GetProfileByDenitst(string dentistId);
        Task<List<ExaminationProfile>> GetProfilesByCustomerId(string customerId);
        Task<List<ExaminationProfile>> GetAllExaminationProfiles();
        Task CreateExaminationProfile(ExaminationProfile examinationProfile);
        Task UpdateExaminationProfile(ExaminationProfile examinationProfile);
        Task DeleteExaminationProfile(int profileId);
    }
}
