using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Repositories.Impl;

public class ClinicRepository : RepositoryBase<Clinic>, IClinicRepository
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
            if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                list = list.Where(x => x.ClinicName.Contains(filterQuery));
            }
        }
        if (string.IsNullOrWhiteSpace(sortBy) == false )
        {
            if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                list = isAscending ? list.OrderBy(x => x.ClinicName) : list.OrderByDescending(x => x.ClinicName);
            }else if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
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

    public async Task<Clinic?> UpdateClinic(Clinic clinic)
    {
        Clinic? model = _repositoryContext.Clinics
            .Include(x => x.Rooms)
            .Include(x => x.ClinicServices)
            .FirstOrDefault(x => x.ClinicId == clinic.ClinicId);
            
        if(model != null)
        {
            _repositoryContext.Entry(model).CurrentValues.SetValues(clinic);
            // if new list service, update 
            if (!clinic.ClinicServices.IsNullOrEmpty())
            {
                _repositoryContext.ClinicServices.RemoveRange(model.ClinicServices);
                await _repositoryContext.SaveChangesAsync();
                foreach (var clinicService in clinic.ClinicServices)
                {
                    model.ClinicServices.Add(clinicService);
                }
            }
            await _repositoryContext.SaveChangesAsync();
            return model;
        }
        return null;
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

    public async Task<Clinic> GetClinicByUserId(Guid id)
    {
        var userClinic = await _repositoryContext.Clinics
            .Include(x=>x.ClinicUsers)
            .FirstOrDefaultAsync(x=>x.ClinicUsers.Any(x=>x.UserId == id) &&  x.Status == true);
        return userClinic;
    }
}