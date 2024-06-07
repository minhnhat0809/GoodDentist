using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ClinicUserRepo : RepositoryBase<ClinicUser>, IClinicUserRepo
    {
        public ClinicUserRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }        

        public async Task<ClinicUser?> GetClinicUserByUserAndClinic(string userId, string clinicId)
        {
            List<ClinicUser> clinicUserList = await FindByConditionAsync(cu => cu.ClinicId.Equals(clinicId) && cu.UserId.Equals(userId));

            return clinicUserList.FirstOrDefault();
        }

        public async Task<ClinicUser?> GetClinicUserByUserAndClinicNow(string userId)
        {
            List<ClinicUser> clinicUserList = await FindByConditionAsync(cu => cu.UserId.Equals(Guid.Parse(userId)) && cu.Status == true);

            return clinicUserList.FirstOrDefault();
        }
    }
}
