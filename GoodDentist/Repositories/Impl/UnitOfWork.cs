using BusinessObject;

namespace Repositories.Impl;

public class UnitOfWork : IUnitOfWork
{
    private readonly GoodDentistDbContext _context;
    
    public IAccountRepo Account { get; }
    public IClinicRepository Clinic { get; }
    public IRoleRepo Role { get; }
    public IDentistSlotRepository DentistSlot { get; }

    public UnitOfWork(GoodDentistDbContext context)
    {
        _context = context;
        Account = new AccountRepo(_context);
        Clinic = new ClinicRepository(_context);
        Role = new RoleRepo(_context);
        DentistSlot = new DentistSlotRepository(_context);
    }
    public void Save()
    {
        _context.SaveChanges();
    }
}