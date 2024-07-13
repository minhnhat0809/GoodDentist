using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.PrescriptionDTOs;
using BusinessObject.DTO.PrescriptionDTOs.View;

namespace Services
{
    public interface IPrescriptionService
	{
		Task<ResponseDTO> GetAllPrescription(int pageNumber, int pageSize);

		Task<ResponseDTO> SearchPrescription(string searchValue);

		Task<ResponseDTO> AddPrescription(PrescriptionCreateDTO prescriptionDTO);

		Task<ResponseDTO> UpdatePrescription(PrescriptionUpdateDTO prescriptionDTO);

		Task<ResponseDTO> DeletePrescription(int prescriptionId);

        Task<ResponseDTO> GetPrescriptionDetails(int prescriptionId);
    }
}
