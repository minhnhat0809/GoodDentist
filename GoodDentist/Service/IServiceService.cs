using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;

namespace Services
{
	public interface IServiceService
	{
		Task<ResponseDTO> getAllService(int pageNumber, int rowsPerPage);
		Task<ResponseDTO> createService(CreateServiceDTO serviceDTO);
		Task<ResponseDTO> updateService(CreateServiceDTO model);

	}
}
