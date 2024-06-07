using BusinessObject;
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
    }
}
