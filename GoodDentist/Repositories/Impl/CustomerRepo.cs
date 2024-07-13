using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class CustomerRepo : RepositoryBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<Customer>> GetAllCustomers(int pageNumber, int rowsPerPage)
        {
            return await _repositoryContext.Customers
                .Include(c => c.CustomerClinics)
                .ThenInclude(cc => cc.Clinic )
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            return await _repositoryContext.Customers
                .Include(x => x.CustomerClinics)
                .ThenInclude(x => x.Clinic)
                .FirstOrDefaultAsync(x => x.CustomerClinics.Any(x => x.CustomerId == customerId));
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            await _repositoryContext.Customers.AddAsync(customer);
            await _repositoryContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _repositoryContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer != null)
            {
                _repositoryContext.Entry(existingCustomer).CurrentValues.SetValues(customer);
                _repositoryContext.Customers.Update(existingCustomer);
                await _repositoryContext.SaveChangesAsync();
            }

            return existingCustomer;
        }

        public async Task<Customer> DeleteCustomer(Guid customerId)
        {
            var customer = await _repositoryContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer != null)
            {
                _repositoryContext.Customers.Remove(customer);
                await _repositoryContext.SaveChangesAsync();
            }

            return customer;
        }

        public async Task<Customer> GetCustomerByPhoneOrEmailOrUsername(string input)
        {
            var customer = await _repositoryContext.Customers
                .Where(x => x.PhoneNumber.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                x.Email.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                x.UserName.Equals(input, StringComparison.OrdinalIgnoreCase))
                .Include(x => x.CustomerClinics)
                .FirstOrDefaultAsync();
            return customer;
        }
        public async Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinic(Guid customerId, Guid clinicId)
        {
            return await _repositoryContext.CustomerClinics.FirstOrDefaultAsync(cu => cu.ClinicId.Equals(clinicId)
                && cu.CustomerId.Equals(customerId));
        }
        public async Task<string> GetCustomerName(string customerId)
        {
            var s = await _repositoryContext.Customers.FirstOrDefaultAsync(c => c.CustomerId.Equals(Guid.Parse(customerId)));
            return s.Name;
        }

        public Task<List<Customer>> GetCustomersByClinic(string clinicId)
        {
            return _repositoryContext.Customers
                .Include(c => c.ExaminationProfiles)
                .Include(c => c.CustomerClinics).ThenInclude(cc => cc.Clinic)
                .Where(c => c.CustomerClinics.Any(cc => cc.ClinicId.Equals(Guid.Parse(clinicId)) && cc.Status == true))
                .ToListAsync();
        }
    }
}
