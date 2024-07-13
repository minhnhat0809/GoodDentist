using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICustomerRepo : IRepositoryBase<Customer>
    {
        Task<List<Customer>> GetAllCustomers(int pageNumber, int rowsPerPage);
        Task<Customer> GetCustomerById(Guid customerId);
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<Customer> DeleteCustomer(Guid customerId);
        Task<Customer> GetCustomerByPhoneOrEmailOrUsername(string input);
        Task<CustomerClinic?> GetCustomerClinicByCustomerAndClinic(Guid customerId, Guid clinicId);
        Task<string> GetCustomerName(string customerId);

        Task<List<Customer>> GetCustomersByClinic(string clinicId, int pageNumber, int rowsPerPage);
    }
}