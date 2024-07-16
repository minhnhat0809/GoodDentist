using BusinessObject;
using BusinessObject.Entity;

namespace Repositories;

public interface IClinicRepository : IRepositoryBase<Clinic>
{
    Task<Clinic>  GetClinic(Guid id);
    Task<Clinic> GetClinicByUserId(Guid id);
    Task<List<Clinic>> GetClinics(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true);
    
    Task<Clinic> CreateClinic(Clinic request);
    Task<Clinic> DeleteClinic(Guid id);
    Task<Clinic> UpdateClinic(Clinic request);
    Task<List<Clinic>> GetAllClinics(int pageNumber, int rowsPerPage);
}