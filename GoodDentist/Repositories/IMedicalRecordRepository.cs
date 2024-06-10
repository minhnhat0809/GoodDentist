using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;

namespace Repositories;

public interface IMedicalRecordRepository : IRepositoryBase<MedicalRecord>
{
    Task<MedicalRecord> GetRecord(int id);
    Task<List<MedicalRecord>> GetRecords();
    Task<MedicalRecord> CreateRecord(MedicalRecord record);
    Task<MedicalRecord> DeleteRecord(int id);
    Task<MedicalRecord> UpdateRecord(MedicalRecord record);
}