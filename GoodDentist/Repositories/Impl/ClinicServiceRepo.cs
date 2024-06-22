using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class ClinicServiceRepo : RepositoryBase<ClinicService>, IClinicServiceRepo
    {
		private readonly IDistributedCache distributedCache;
		public ClinicServiceRepo(GoodDentistDbContext repositoryContext, IDistributedCache distributedCache) : base(repositoryContext)
        {
			this.distributedCache = distributedCache;
		}

		public async Task<ResponseDTO> CheckValidate(ClinicServiceDTO clinicServiceDTO)
		{
			ClinicService clinicService= _repositoryContext.ClinicServices.Where(c=>c.ClinicId.Equals(clinicServiceDTO.ClinicId)).FirstOrDefault();
			if (clinicService != null)
				return new ResponseDTO("Duplicated", 400, false, null);
			return new ResponseDTO("Sucess", 200, true, null);

		}

		public async Task<List<ClinicService>> GetAllClinicService(int pageNumber, int rowsPerPage)
		{
			List<ClinicService> clinicServices = await Paging(pageNumber, rowsPerPage);
			clinicServices.Where(s => true).ToList();
			return clinicServices;
		}

	}
}
