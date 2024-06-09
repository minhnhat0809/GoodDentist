using BusinessObject;
using BusinessObject.Entity;

namespace Repositories;

public interface IDentistSlotRepository : IRepositoryBase<DentistSlot>
{
    Task<DentistSlot> GetDentistSlot(int id);
    Task<List<DentistSlot>> GetDentistSlots();
}