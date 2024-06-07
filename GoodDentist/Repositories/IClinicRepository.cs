using BusinessObject;

namespace Repositories;

public interface IClinicRepository : IRepositoryBase<Clinic>
{
    Task<Clinic>  GetClinic(Guid id);
    Task<List<Clinic>> GetClinics();
}