using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ClinicServiceDTOs.View;
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
		
		public ClinicServiceRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
			
		}

		public async Task<ResponseDTO> CheckValidate(ClinicServiceDTO clinicServiceDTO)
		{
			ClinicService? clinicService= _repositoryContext.ClinicServices.Where(c=>c.ClinicId.Equals(clinicServiceDTO.ClinicServiceId) && c.ServiceId.Equals(clinicServiceDTO.ServiceId)).FirstOrDefault();
			if (clinicService != null)
				return new ResponseDTO("Duplicated", 400, false, null);
			return new ResponseDTO("Sucess", 200, true, null);

		}

		public async Task<List<ClinicService>> GetAllClinicService(int pageNumber, int rowsPerPage)
		{
			List<ClinicService> clinicServices = await Paging(pageNumber, rowsPerPage);
			clinicServices.Where(s => true && s.Status==true).ToList();
			return clinicServices;
		}

		public async Task<ClinicService> GetClinicServiceByID(int clinicServiceId)
		{
			ClinicService clinicService = _repositoryContext.ClinicServices.Where(c => c.ClinicServiceId.Equals(clinicServiceId)).FirstOrDefault();
			return clinicService;
		}
	}
}
