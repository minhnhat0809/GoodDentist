using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IExaminationRepo : IRepositoryBase<Examination>
    {
        Task<Examination?> GetExaminationById(int examId);

        Task<List<Examination>> GetAllExaminationOfDentist(string clinicId, string userId, DateOnly selectedDate, int pageNumber, int rowsPerpage);

        Task<List<Examination>> GetAllExaminationOfCustomer(string clinicId, string userId, int pageNumber, int rowsPerpage);

        Task<List<Examination>> GetAllExaminationOfClinic(string clinicId, int pageNumber, int rowsPerpage);

        Task<List<Examination>> GetExaminationByProfileId(int profileId, int pageNumber, int rowsPerpage);
        Task<List<Examination>> GetAllExaminationOfDentistSlot(int dentistSlotId);
    }
}
