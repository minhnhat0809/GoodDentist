using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories;

namespace Services.Impl;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFirebaseStorageService _firebaseStorageService;

    public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService) 
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _firebaseStorageService = firebaseStorageService;
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

    public async Task<MedicalRecordDTO> UploadFile(IFormFile file, int recordId)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(recordId);
        if (model == null)
        {
            throw new Exception("Medical record not found.");
        }

        if (model.Url != null)
        {
            // Delete the image before add new one
            await _firebaseStorageService.DeleteFileAndReference(model.Url);
        }

        // Generate a unique file name
        var fileName = $"{model.MedicalRecordId}-{Guid.NewGuid()}";
        
        // Upload image to Firebase Storage
        var url = await _firebaseStorageService.UploadFile(fileName, file, "medical-record");

        // Update the URL in the medical record model
        model.Url = url;
        model = await _unitOfWork.MedicalRecordRepository.UpdateRecord(model);
        
        return _mapper.Map<MedicalRecordDTO>(model);
    }
    
    public async Task<MedicalRecordDTO> DeleteFileAndReference(int recordId)
    {
        var model = await _unitOfWork.MedicalRecordRepository.GetRecord(recordId);
        if (model == null)
        {
            throw new Exception("Medical record not found.");
        }
        // Delete image to Firebase Storage
        await _firebaseStorageService.DeleteFileAndReference(model.Url);

        // Update the URL in the medical record model;
        model.Url = null;
        model = await _unitOfWork.MedicalRecordRepository.UpdateRecord(model);
        
        return _mapper.Map<MedicalRecordDTO>(model);
    }
    
    public async Task<ResponseDTO> CreateRecordTest(MedicalRecordRequestTestDTO record)
    {
        ResponseDTO responseDTO = new ResponseDTO("Upload Customer File Successfully", 200, true, null);
        try
        {
            var model = await _unitOfWork.MedicalRecordRepository.GetRecord(record.MedicalRecordId);
            if (model == null)
            {
                var createModel = _mapper.Map<MedicalRecord>(record);
                model = await _unitOfWork.MedicalRecordRepository.CreateRecord(createModel);
                if (model.MedicalRecordId != null)
                {
                    if (model == null)
                    {
                        throw new Exception("Medical record not found.");
                    }

                    if (model.Url != null)
                    {
                        // Delete the image before add new one
                        await _firebaseStorageService.DeleteFileAndReference(model.Url);
                    }

                    // Generate a unique file name
                    var fileName = $"{model.MedicalRecordId}-{Guid.NewGuid()}";

                    // Upload image to Firebase Storage
                    var url = await _firebaseStorageService.UploadFile(fileName, record.UploadFile, "medical-record");

                    // Update the URL in the medical record model
                    model.Url = url;
                    model = await _unitOfWork.MedicalRecordRepository.UpdateRecord(model);
                    responseDTO.Result = _mapper.Map<MedicalRecordDTO>(model);
                }
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return responseDTO;
    }
}