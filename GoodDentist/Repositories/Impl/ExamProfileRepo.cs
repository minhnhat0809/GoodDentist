using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<ExaminationProfile>> GetProfileByDenitst(string dentistId)
        {
            return await _repositoryContext.ExaminationProfiles
                .Include(ex => ex.Customer)
                .Where(e => e.DentistId.Equals(Guid.Parse(dentistId))).ToListAsync();
        }

        public async Task<List<ExaminationProfile>> GetProfilesByCustomerId(string customerId)
        {
            return await _repositoryContext.ExaminationProfiles
                .Include(ex => ex.Examinations).ThenInclude(e => e.MedicalRecords)
                .Include(ex => ex.Examinations).ThenInclude(e => e.Orders)
                .Include(ex => ex.Examinations).ThenInclude(e => e.Prescriptions)
                .Where(ex => ex.CustomerId.Equals(Guid.Parse(customerId))).ToListAsync();
        }

        // Get all examination profiles
        public async Task<List<ExaminationProfile>> GetAllExaminationProfiles()
        {
            return await _repositoryContext.ExaminationProfiles.ToListAsync();
        }

        // Create a new examination profile
        public async Task CreateExaminationProfile(ExaminationProfile examinationProfile)
        {
            await _repositoryContext.ExaminationProfiles.AddAsync(examinationProfile);
            await _repositoryContext.SaveChangesAsync();
        }

        // Update an existing examination profile
        public async Task UpdateExaminationProfile(ExaminationProfile examinationProfile)
        {
            _repositoryContext.ExaminationProfiles.Update(examinationProfile);
            await _repositoryContext.SaveChangesAsync();
        }

        // Delete an examination profile
        public async Task DeleteExaminationProfile(int id)
        {
            var examinationProfile = await GetExaminationProfileById(id);
            if (examinationProfile != null)
            {
                _repositoryContext.ExaminationProfiles.Remove(examinationProfile);
                await _repositoryContext.SaveChangesAsync();
            }
        }
    }
}
