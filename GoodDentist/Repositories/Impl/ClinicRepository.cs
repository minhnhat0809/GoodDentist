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

    public async Task<List<Clinic>> GetClinics(
        string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true)

    {
        var list = _repositoryContext.Clinics.AsQueryable();
        // Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("ClinicName", StringComparison.OrdinalIgnoreCase))
            {
                list = list.Where(x => x.ClinicName.Contains(filterQuery));
            }
        }
        if (string.IsNullOrWhiteSpace(sortBy) == false )
        {
            if (sortBy.Equals("ClinicName", StringComparison.OrdinalIgnoreCase))
            {
                list = isAscending ? list.OrderBy(x => x.ClinicName) : list.OrderByDescending(x => x.ClinicName);
            }else if (sortBy.Equals("ClinicName", StringComparison.OrdinalIgnoreCase))
            {
                list = isAscending ? list.OrderBy(x => x.Address) : list.OrderByDescending(x => x.Address);
            }
        }
        return await list.ToListAsync();
    }

    public async Task<Clinic> CreateClinic(Clinic clinic)
    {
        _repositoryContext.Clinics.Add(clinic);
        await _repositoryContext.SaveChangesAsync();
        return clinic;
    }

    public async Task<Clinic> UpdateClinic(Clinic clinic)
    {
        _repositoryContext.Entry(clinic).State = EntityState.Modified;
        await _repositoryContext.SaveChangesAsync();
        return clinic;
    }

    public async Task<Clinic> DeleteClinic(Guid id)
    {
        var clinic = await _repositoryContext.Clinics.FindAsync(id);
        if (clinic == null)
        {
            return null;
        }

        _repositoryContext.Clinics.Remove(clinic);
        await _repositoryContext.SaveChangesAsync();
        return clinic;
    }
}