using BusinessObject;

namespace Repositories;

public interface IDentistSlotRepository : IRepositoryBase<DentistSlot>
{
    DentistSlot GetDentistSlot(int id);
    List<DentistSlot> GetDentistSlots();
}