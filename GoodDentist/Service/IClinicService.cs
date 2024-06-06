using BusinessObject;

namespace Services;

public interface IClinicService
{
    Task<Clinic> GetClinic(Guid id);
    Task<List<Clinic>> GetClinics();
}