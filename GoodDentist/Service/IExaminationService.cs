using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
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
        Task<ResponseDTO> GetExaminationById(int examId);

        Task<ResponseListDTO> CreateExamination(ExaminationDTO examinationDTO);
    }
}
