using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;

namespace Services
{
    public interface IExaminationService
    {
        Task<ResponseDTO> GetExamination(int id);

        Task<ResponseDTO> GetExaminations();

        Task<ResponseListDTO> CreateExamination(ExaminationRequestDTO requestDto);
        Task<ResponseDTO> DeleteExamination(int id);
        Task<ResponseListDTO> UpdateExamination(ExaminationRequestDTO requestDto);
        Task<ResponseDTO> GetAllExaminationsOfClinic(Guid clinicId);
        Task<ResponseDTO> GetAllExaminationsOfUser(Guid userId);
    }
}
