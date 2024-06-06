using BusinessObject;

namespace Services;

public interface IDentistSlotService
{
    Task<DentistSlot> GetDentistSlot(int id);
    Task<List<DentistSlot>> GetDentistSlots();
}