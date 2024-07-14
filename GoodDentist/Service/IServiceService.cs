using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.ServiceDTOs;

namespace Services
{
    public interface IServiceService
	{
		Task<ResponseDTO> getAllService(int pageNumber, int rowsPerPage);
		Task<ResponseDTO> createService(CreateServiceDTO serviceDTO);
		Task<ResponseDTO> updateService(CreateServiceDTO model);
		Task<ResponseDTO> deleteService(int serviceID);

		Task<ResponseDTO> GetAllServicesByClinic(string clinicId, string? filterField, string? filterValue,
			int? pageNumber,
			int? rowsPerPage);
	}
}
