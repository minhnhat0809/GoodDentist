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
            return await _repositoryContext.ExaminationProfiles
                .Include(ep => ep.Customer)
                .Include(ep => ep.Dentist)
                .FirstOrDefaultAsync(ep => ep.ExaminationProfileId == id);
        }

        public async Task<List<ExaminationProfile>> GetProfileByDenitst(string dentistId)
        {
            return await _repositoryContext.ExaminationProfiles
                .Include(ex => ex.Customer).ThenInclude(c => c.CustomerClinics).ThenInclude(cc => cc.Clinic)
                .Where(e => e.DentistId.Equals(Guid.Parse(dentistId))).ToListAsync();
        }

        public async Task<List<ExaminationProfile>> GetProfilesByCustomerId(string customerId)
        {
            return await _repositoryContext.ExaminationProfiles
                .Include(ep => ep.Customer)
                .Include(ep => ep.Dentist)
                .Include(ex => ex.Examinations)
                    .ThenInclude(e => e.MedicalRecords).ThenInclude(mr => mr.RecordType)
                .Include(ex => ex.Examinations)
                    .ThenInclude(e => e.Orders).ThenInclude(o => o.OrderServices).ThenInclude(os => os.Service)
                .Include(ex => ex.Examinations).ThenInclude(e => e.Prescriptions)
                    .ThenInclude(p => p.MedicinePrescriptions).ThenInclude(mp => mp.Medicine)
                .Include(ex => ex.Examinations).ThenInclude(e => e.Dentist)
                .Where(ex => ex.CustomerId.Equals(Guid.Parse(customerId))).ToListAsync();
        }
    }
}
