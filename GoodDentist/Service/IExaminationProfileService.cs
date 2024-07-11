using BusinessObject.DTO;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;

namespace Services
{
    public interface IExaminationProfileService
    {
        Task<ResponseDTO> GetExaminationProfilesByCustomerId(string customerId);
        Task<ResponseDTO> GetAllExaminationProfiles();
        Task<ResponseDTO> CreateExaminationProfile(ExaminationProfileDTO examinationProfileDTO);
        Task<ResponseDTO> UpdateExaminationProfile(ExaminationProfileDTO examinationProfileDTO);
        Task<ResponseDTO> DeleteExaminationProfile(int id);
    }
}