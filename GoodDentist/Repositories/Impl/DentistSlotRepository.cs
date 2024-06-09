using System.Linq.Expressions;
using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

public class DentistSlotRepository : RepositoryBase<DentistSlot>, IDentistSlotRepository
{
    private readonly GoodDentistDbContext _context;

    public DentistSlotRepository(GoodDentistDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<DentistSlot> GetDentistSlot(int id)
    {
        return await _context.DentistSlots.FindAsync(id);
    }

    public async Task<List<DentistSlot>> GetDentistSlots()
    {
        return await _context.DentistSlots.ToListAsync();
    }
}