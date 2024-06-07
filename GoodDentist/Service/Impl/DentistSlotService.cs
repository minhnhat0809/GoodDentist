using AutoMapper;
using BusinessObject;
using BusinessObject.DTO.ViewDTO;
using Repositories;

namespace Services.Impl;

public class DentistSlotService : IDentistSlotService
{
    private readonly IDentistSlotRepository _dentistSlotRepository;
    private readonly IMapper _mapper;

    public DentistSlotService(IDentistSlotRepository dentistSlotRepository, IMapper mapper)
    {
        _dentistSlotRepository = dentistSlotRepository;
        _mapper = mapper;
    }

    public async Task<DentistSlotDTO> GetDentistSlot(int id)
    {
        var model  = await _dentistSlotRepository.GetDentistSlot(id);
        return _mapper.Map<DentistSlotDTO>(model);
    }

    public async Task<List<DentistSlotDTO>> GetDentistSlots()
    {
        var list = await _dentistSlotRepository.GetDentistSlots();
        return _mapper.Map<List<DentistSlotDTO>>(list);
    }
}