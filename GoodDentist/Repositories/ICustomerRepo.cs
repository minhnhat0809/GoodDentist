using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;

namespace Repositories
{
    public interface ICustomerRepo : IRepositoryBase<Customer>
    {
        Task<List<Customer>> GetAllCustomers(int pageNumber, int rowsPerPage);
        Task<Customer> GetCustomerById(Guid customerId);
        Task CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Guid customerId);
        Task<Customer> GetCustomerByPhoneOrEmailOrUsername(string input);
    }
}