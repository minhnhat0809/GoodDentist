using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;

namespace Services
{
	public interface IClinicServiceService
	{
		Task<ResponseDTO> CreateClinicService(ClinicServiceDTO clinicServiceDTO);
		Task<ResponseDTO> getAllClinicService(int pageNumber, int rowsPerPage);
	}
}
