using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.ClinicServiceDTOs.View;

namespace Services
{
    public interface IClinicServiceService
	{
		Task<ResponseDTO> CreateClinicService(ClinicServiceDTO clinicServiceDTO);
		Task<ResponseDTO> DeleteClinicService(int clinicServiceID);
		Task<ResponseDTO> getAllClinicService(int pageNumber, int rowsPerPage);
		Task<ResponseDTO> UpdateClinicService(ClinicServiceDTO model);
	}
}
