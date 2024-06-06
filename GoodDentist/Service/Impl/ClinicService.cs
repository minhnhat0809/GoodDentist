using BusinessObject;
using Repositories;

namespace Services.Impl;

public class ClinicService : IClinicService
{
    private readonly IClinicRepository _clinicRepository;

    public ClinicService(IClinicRepository clinicRepository)
    {
        _clinicRepository = clinicRepository;
    }

    public async Task<Clinic> GetClinic(Guid id)
    {
        return _clinicRepository.GetClinic(id);
    }

    public Task<List<Clinic>> GetClinics()
    {
        return _clinicRepository.FindAllAsync();
    }
}