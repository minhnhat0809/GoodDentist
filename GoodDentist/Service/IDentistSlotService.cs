using BusinessObject;
using BusinessObject.DTO.ViewDTO;

namespace Services;

public interface IDentistSlotService
{
    Task<DentistSlotDTO> GetDentistSlot(int id);
    Task<List<DentistSlotDTO>> GetDentistSlots();
}