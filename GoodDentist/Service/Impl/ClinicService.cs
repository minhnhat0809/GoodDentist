using AutoMapper;
using BusinessObject;
using BusinessObject.DTO.ViewDTO;
using Repositories;

namespace Services.Impl;

public class ClinicService : IClinicService
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IMapper _mapper;

    public ClinicService(IClinicRepository clinicRepository, IMapper mapper)
    {
        _clinicRepository = clinicRepository;
        _mapper = mapper;
    }

    public async Task<ClinicDTO> GetClinic(Guid id)
    {
        var clinicModel = await _clinicRepository.GetClinic(id);
        return _mapper.Map<ClinicDTO>(clinicModel);
        //return clinicModel;
    }
 
    public async Task<List<ClinicDTO>> GetClinics()
    {
        var list = await _clinicRepository.GetClinics();
        var listDto =  _mapper.Map<List<ClinicDTO>>(list);
        return listDto;
    }
}