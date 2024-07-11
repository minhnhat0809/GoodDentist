using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;

namespace Services
{
    public interface ICustomerService
    {
        Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId);
        Task<ResponseDTO> GetAllCustomers(string search, int pageNumber, int rowsPerPage);
        Task<ResponseDTO> GetCustomerById(string customerId);
        Task<ResponseDTO> DeleteCustomer(Guid userId);
        Task<ResponseDTO> UpdateCustomer(CustomerDTO customerDto);
        Task<ResponseDTO> CreateCustomer(CustomerDTO customerDto);
    }
}