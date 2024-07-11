using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
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
                .Include(c => c.CustomerClinics).ThenInclude(cc => cc.Clinic)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerById(Guid customerId)
        {
            return await _repositoryContext.Customers
                .Include(c => c.CustomerClinics).ThenInclude(cc => cc.Clinic)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task CreateCustomer(Customer customer)
        {
            await _repositoryContext.Customers.AddAsync(customer);
            await _repositoryContext.SaveChangesAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var existingCustomer = await _repositoryContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

            if (existingCustomer != null)
            {
                // Update fields as necessary
                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.Address = customer.Address;
                // ... any other fields to update
                
                _repositoryContext.Customers.Update(existingCustomer);
                await _repositoryContext.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            var customer = await _repositoryContext.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer != null)
            {
                _repositoryContext.Customers.Remove(customer);
                await _repositoryContext.SaveChangesAsync();
            }
        }
    }
}
