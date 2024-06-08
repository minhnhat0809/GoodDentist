using AutoMapper;
using BusinessObject;
using BusinessObject.DTO.ViewDTO;
using Repositories;

namespace Services.Impl;

public class ClinicService : IClinicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClinicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ClinicDTO> GetClinic(Guid id)
    {
        var clinicModel = await _unitOfWork.ClinicRepository.GetClinic(id);
        return _mapper.Map<ClinicDTO>(clinicModel);
        //return clinicModel;
    }
 
    public async Task<List<ClinicDTO>> GetClinics(string? filterOn , string? filterQuery, string? sortBy, bool isAscending )
    {
        var list = await _unitOfWork.ClinicRepository.GetClinics(filterOn, filterQuery, sortBy, isAscending);
        var listDto =  _mapper.Map<List<ClinicDTO>>(list);
        return listDto;
    }

    public async Task<ClinicDTO> CreateClinic(ClinicRequestDTO requestDto)
    {
        var clinic = _mapper.Map<Clinic>(requestDto);
        var createdClinic = await _unitOfWork.ClinicRepository.CreateClinic(clinic);
        return _mapper.Map<ClinicDTO>(createdClinic);
    }

    public async Task<ClinicDTO> DeleteClinic(Guid id)
    {
        var clinic = await _unitOfWork.ClinicRepository.DeleteClinic(id);
        if (clinic == null)
        {
            return null; // Or handle not found scenario
        }
        return _mapper.Map<ClinicDTO>(clinic);
    }

    public async Task<ClinicDTO> UpdateClinic(ClinicRequestDTO requestDto)
    {
        var clinic = _mapper.Map<Clinic>(requestDto);
        var updatedClinic = await _unitOfWork.ClinicRepository.UpdateClinic(clinic);
        return _mapper.Map<ClinicDTO>(updatedClinic);
    }
}