using BusinessObject.DTO;
using BusinessObject.DTO.ClinicServiceDTOs.View;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClinicServiceRepo : IRepositoryBase<ClinicService>
	{
		Task<ResponseDTO> CheckValidate(ClinicServiceDTO clinicServiceDTO);
		Task<List<ClinicService>> GetAllClinicService(int pageNumber, int rowsPerPage);
		Task<ClinicService> GetClinicServiceByID(int clinicServiceId);
	}
}
