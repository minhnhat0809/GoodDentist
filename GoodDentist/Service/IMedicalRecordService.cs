using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;

namespace Services;

public interface IMedicalRecordService
{
    Task<MedicalRecordDTO> GetRecord(int id);
    Task<List<MedicalRecordDTO>> GetRecords();
    Task<MedicalRecordDTO> CreateRecord(MedicalRecordRequestDTO record);
    Task<MedicalRecordDTO> DeleteRecord(int id);
    Task<MedicalRecordDTO> UpdateRecord(MedicalRecordRequestDTO record);
}