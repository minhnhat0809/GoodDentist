using BusinessObject.DTO;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;

namespace Services
{
    public interface IExaminationProfileService
    {
        Task<ResponseDTO> GetExaminationProfilesByCustomerId(string customerId);
        Task<ResponseDTO> GetAllExaminationProfiles();
        Task<ResponseDTO> CreateExaminationProfile(ExaminationProfileForExamDTO examinationProfileDTO);
        Task<ResponseDTO> UpdateExaminationProfile(ExaminationProfileForExamDTO examinationProfileDTO);
        Task<ResponseDTO> DeleteExaminationProfile(int id);
    }
}