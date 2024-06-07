using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

public class ClinicRepository: RepositoryBase<Clinic>, IClinicRepository
{
    private readonly GoodDentistDbContext _context;
    
    public ClinicRepository( GoodDentistDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Clinic> GetClinic(Guid id)
    {
        var clinic = await _context.Clinics.FindAsync(id);
        return clinic;
    }

    public async Task<List<Clinic>> GetClinics()
    {
        return await _context.Clinics.ToListAsync();
    }
}