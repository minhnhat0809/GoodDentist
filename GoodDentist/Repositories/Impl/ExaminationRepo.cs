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

        public async Task<List<Examination>> GetAllExaminationOfClinic(string clinicId, int pageNumber, int rowsPerpage)
        {
            return await _repositoryContext.Examinations.Where(ex => ex.DentistSlot.Room.ClinicId.Equals(Guid.Parse(clinicId)))
                .Skip((pageNumber - 1) * rowsPerpage)
                .Take(rowsPerpage)
                .ToListAsync();
        }

        public async Task<List<Examination>> GetAllExaminationOfCustomer(string clinicId, string userId, int pageNumber, int rowsPerpage)
        {
            return await _repositoryContext.Examinations.Where(ex => ex.ExaminationProfile.CustomerId.Equals(Guid.Parse(userId)) 
            && ex.DentistSlot.Room.ClinicId.Equals(Guid.Parse(clinicId)))
                .Skip((pageNumber - 1) * rowsPerpage)
                .Take(rowsPerpage)
                .ToListAsync();
        }

        public async Task<List<Examination>> GetAllExaminationOfDentist(string clinicId, string userId, int pageNumber, int rowsPerpage)
        {
            return await _repositoryContext.Examinations.Where(ex => ex.DentistId.Equals(Guid.Parse(userId)) 
            && ex.DentistSlot.Room.ClinicId.Equals(Guid.Parse(clinicId)))
                .Skip((pageNumber - 1) * rowsPerpage)
                .Take(rowsPerpage)
                .ToListAsync();
        }

        public async Task<List<Examination>> GetAllExaminationOfDentistSlot(int dentistSlotId)
        {
            return await _repositoryContext.Examinations.Where(ex => ex.DentistSlotId == dentistSlotId).ToListAsync();
        }

        public async Task<Examination?> GetExaminationById(int examId)
        {
            Examination? examination = await _repositoryContext.Examinations.FirstOrDefaultAsync(ex => ex.ExaminationId == examId);

            return examination;
        }

        public async Task<List<Examination>> GetExaminationByProfileId(int profileId, int pageNumber, int rowsPerpage )
        {
            return await _repositoryContext.Examinations.Where(ex => ex.ExaminationProfileId == profileId)
                .Skip((pageNumber - 1) * rowsPerpage)
                .Take(rowsPerpage)
                .ToListAsync();
        }
    }
}
