using BusinessObject;
using BusinessObject.Entity;

namespace Repositories.Impl;

public class MedicalRecordRepository : RepositoryBase<MedicalRecord>,IMedicalRecordRepository
{
    public MedicalRecordRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<MedicalRecord> GetRecord(int id)
    {
        var exist = await  _repositoryContext.FindAsync<MedicalRecord>(id);
        return exist;
    }

    public Task<List<MedicalRecord>> GetRecords()
    {
        var exist =  FindAllAsync();
        return exist;
    }

    public async Task<MedicalRecord> CreateRecord(MedicalRecord record)
    {
        await _repositoryContext.MedicalRecords.AddAsync(record);
        await _repositoryContext.SaveChangesAsync();
        return record;
    }

    public async Task<MedicalRecord> DeleteRecord(int id)
    {
        var record = await _repositoryContext.FindAsync<MedicalRecord>(id);
        if (record != null)
        {
            _repositoryContext.MedicalRecords.Remove(record);
            await _repositoryContext.SaveChangesAsync();
        }
        return record;
    }

    public async Task<MedicalRecord> UpdateRecord(MedicalRecord record)
    {
        var existingRecord = await _repositoryContext.FindAsync<MedicalRecord>(record.MedicalRecordId);
        if (existingRecord != null)
        {
            _repositoryContext.Entry(existingRecord).CurrentValues.SetValues(record);
            await _repositoryContext.SaveChangesAsync();
        }
        return existingRecord;
    }
}