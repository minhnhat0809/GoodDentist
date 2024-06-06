using BusinessObject;
using Repositories;

namespace Services.Impl;

public class DentistSlotService : IDentistSlotService
{
    private readonly IDentistSlotRepository _dentistSlotRepository;

    public DentistSlotService(IDentistSlotRepository dentistSlotRepository)
    {
        _dentistSlotRepository = dentistSlotRepository;
    }

    public async Task<DentistSlot> GetDentistSlot(int id)
    {
        return _dentistSlotRepository.GetDentistSlot(id);
    }

    public async Task<List<DentistSlot>> GetDentistSlots()
    {
        return _dentistSlotRepository.GetDentistSlots();
    }
}