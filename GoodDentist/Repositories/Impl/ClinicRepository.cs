using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

public class ClinicRepository: RepositoryBase<Clinic>, IClinicRepository
{
    
    public ClinicRepository( GoodDentistDbContext context) : base(context)
    {
        
    }

    public async Task<Clinic> GetClinic(Guid id)
    {
        var clinic = await _repositoryContext.Clinics.FindAsync(id);
        return clinic;
    }

    public async Task<List<Clinic>> GetClinics()
    {
        var list = await _repositoryContext.Clinics.ToListAsync();
        return list;
    }
}