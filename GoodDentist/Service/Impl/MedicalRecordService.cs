using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories;

namespace Services.Impl;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<MedicalRecordDTO> GetRecord(int id)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(id);
        return _mapper.Map<MedicalRecordDTO>(model);
    }

    public async Task<List<MedicalRecordDTO>> GetRecords()
    {
        var models = await _unitOfWork.MedicalRecordRepository.GetRecords();
        return _mapper.Map<List<MedicalRecordDTO>>(models);
    }

    public async Task<MedicalRecordDTO> CreateRecord(MedicalRecordRequestDTO record)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(record.MedicalRecordId);
        if (model == null)
        {
            var createModel = _mapper.Map<MedicalRecord>(record);
            model = await _unitOfWork.MedicalRecordRepository.CreateRecord(createModel);
        }
        var dto = _mapper.Map<MedicalRecordDTO>(model);
        return dto;
    }

    public async Task<MedicalRecordDTO> DeleteRecord(int id)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(id);
        if (model != null)
        {
            model = await _unitOfWork.MedicalRecordRepository.DeleteRecord(id);
        }
        var dto = _mapper.Map<MedicalRecordDTO>(model);
        return dto;
    }

    public async Task<MedicalRecordDTO> UpdateRecord(MedicalRecordRequestDTO record)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(record.MedicalRecordId);
        if (model != null)
        {
            var updateModel = _mapper.Map<MedicalRecord>(record);
            model = await _unitOfWork.MedicalRecordRepository.UpdateRecord(updateModel);
        }
        var dto = _mapper.Map<MedicalRecordDTO>(model);
        return dto;
    }
}