using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.ClinicDTOs;
using BusinessObject.DTO.ClinicDTOs.View;

namespace Services;

public interface IClinicService
{
    Task<ClinicDTO>  GetClinic(Guid id);
    Task<ClinicDTO> GetClinicByUserId(Guid userId);
    Task<List<ClinicDTO>> GetClinics(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true);

    Task<ResponseDTO> CreateClinic(ClinicCreateDTO requestDto);
    Task<ResponseDTO> DeleteClinic(Guid id);
    Task<ResponseDTO> UpdateClinic(ClinicUpdateDTO requestDto);
}