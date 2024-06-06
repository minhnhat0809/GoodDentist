using System.Linq.Expressions;
using BusinessObject;

namespace Repositories.Impl;

public class DentistSlotRepository : RepositoryBase<DentistSlot>, IDentistSlotRepository
{
    private readonly GoodDentistDbContext _context;

    public DentistSlotRepository(GoodDentistDbContext context) : base(context)
    {
        _context = context;
    }


    public DentistSlot GetDentistSlot(int id)
    {
        return _context.DentistSlots.Find(id);
    }

    public List<DentistSlot> GetDentistSlots()
    {
        return _context.DentistSlots.ToList();
    }
}