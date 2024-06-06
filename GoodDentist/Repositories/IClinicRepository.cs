using BusinessObject;

namespace Repositories;

public interface IClinicRepository : IRepositoryBase<Clinic>
{
    Clinic GetClinic(Guid id);
    List<Clinic> GetClinics();
}