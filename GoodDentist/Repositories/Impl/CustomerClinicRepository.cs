using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl
{
    public interface ICustomerClinicRepository : IRepositoryBase<CustomerClinic>
    {
        
        Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinic(string customerId, string clinicId);
        Task<List<CustomerClinic>> GetCustomerClinicsByCustomer(string customerId);
        Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinicNow(string userId);
    }
    
    public class CustomerClinicRepository : RepositoryBase<CustomerClinic>, ICustomerClinicRepository
    {
        public CustomerClinicRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public async Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinic(string customerId, string clinicId)
        {
            var customerIdGuid = Guid.Parse(customerId);
            var clinicIdGuid = Guid.Parse(clinicId);
            return await _repositoryContext.CustomerClinics
                .FirstOrDefaultAsync(cc => cc.CustomerId.Equals(customerIdGuid) && cc.ClinicId.Equals(clinicIdGuid));
        }

        public async Task<List<CustomerClinic>> GetCustomerClinicsByCustomer(string customerId)
        {
            var customerIdGuid = Guid.Parse(customerId);
            return await _repositoryContext.CustomerClinics
                .Where(cc => cc.CustomerId.Equals(customerIdGuid))
                .ToListAsync();
        }

        public async Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinicNow(string customerId)
        {
            List<CustomerClinic> customerClinics = await FindByConditionAsync(cu => cu.CustomerId.Equals(Guid.Parse(customerId)) && cu.Status == true);

            return customerClinics.FirstOrDefault();
            
        }
    }
}
    

