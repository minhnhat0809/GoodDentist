using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface ICustomerService
    {
        Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId, string search);
        Task<ResponseDTO> GetAllCustomers(string search, int pageNumber, int rowsPerPage);
        Task<ResponseDTO> GetCustomerById(string customerId);
        Task<ResponseDTO> DeleteCustomer(Guid userId);
        Task<ResponseDTO> UpdateCustomer(CustomerRequestDTO customerDto);
        Task<ResponseDTO> CreateCustomer(CustomerRequestDTO customerDto);
        Task<ResponseDTO> UploadFile(IFormFile file, Guid customerId);
        Task<ResponseDTO> DeleteFileAndReference(Guid customerId);
    }
}