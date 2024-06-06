using BusinessObject;

namespace Repositories.Impl;

public class ClinicRepository: RepositoryBase<Clinic>, IClinicRepository
{
    private readonly GoodDentistDbContext _context;
    
    public ClinicRepository( GoodDentistDbContext context) : base(context)
    {
        _context = context;
    }

    public Clinic GetClinic(Guid id)
    {
        var clinic =  _context.Clinics.Find(id);
        return clinic;
    }

    public List<Clinic> GetClinics()
    {
        return _context.Clinics.ToList();
    }
}