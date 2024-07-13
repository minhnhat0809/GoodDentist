using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;
using BusinessObject.DTO.CustomerDTOs;

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
        Task<ResponseDTO> GetCustomers(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField, string? sortOrder);
        Task<ResponseListDTO> updateCustomer(CustomerRequestDTO customerRequestDto);
        Task<ResponseDTO> GetCustomersByClinic(string clinicId);
    }
}