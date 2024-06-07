using BusinessObject;
using BusinessObject.DTO.ViewDTO;

namespace Services;

public interface IClinicService
{
    Task<ClinicDTO>  GetClinic(Guid id);
    Task<List<ClinicDTO>> GetClinics();
}