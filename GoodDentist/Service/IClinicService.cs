using BusinessObject;
using BusinessObject.DTO.ViewDTO;

namespace Services;

public interface IClinicService
{
    Task<ClinicDTO>  GetClinic(Guid id);

    Task<List<ClinicDTO>> GetClinics(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true);

    Task<ClinicDTO> CreateClinic(ClinicRequestDTO requestDto);
    Task<ClinicDTO> DeleteClinic(Guid id);
    Task<ClinicDTO> UpdateClinic(ClinicRequestDTO requestDto);
}