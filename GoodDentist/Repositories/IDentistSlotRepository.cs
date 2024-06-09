using BusinessObject;
using BusinessObject.Entities;

namespace Repositories;

public interface IDentistSlotRepository : IRepositoryBase<DentistSlot>
{
    Task<DentistSlot> GetDentistSlot(int id);
    Task<List<DentistSlot>> GetDentistSlots();
}