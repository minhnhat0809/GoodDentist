using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services;

public interface IMedicalRecordService
{
    Task<MedicalRecordDTO> GetRecord(int id);
    Task<List<MedicalRecordDTO>> GetRecords();
    Task<MedicalRecordDTO> CreateRecord(MedicalRecordRequestDTO record);
    Task<MedicalRecordDTO> DeleteRecord(int id);
    Task<MedicalRecordDTO> UpdateRecord(MedicalRecordRequestDTO record);
    Task<MedicalRecordDTO> UploadFile(IFormFile file, int recordId);
    Task<MedicalRecordDTO> DeleteFileAndReference(int recordId);
    Task<ResponseDTO> CreateRecordTest(MedicalRecordRequestTestDTO record);
}