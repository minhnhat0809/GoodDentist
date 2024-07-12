using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClinicUserRepo : IRepositoryBase<ClinicUser>
    {
        Task<ClinicUser?> GetClinicUserByUserAndClinic(string userId, string clinicId);

        Task<ClinicUser?> GetClinicUserByUserAndClinicNow(string userId);

        Task<List<User>> GetAllUsersByClinic(string clinicId, int pageNumber, int rowsPerPage);

        Task<ClinicUser?> GetClinicUserByUserAndClinicc(string userId, string clinicId);

    }
}
