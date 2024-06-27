using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {

        Task<ResponseListDTO> createUser(CreateUserDTO createUserDTO);

        Task<ResponseDTO> getAllUsers(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField, string? sortOrder);

        Task<ResponseDTO> getAllUsersByClinic(string clinicId, int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField, string? sortOrder);

        Task<ResponseDTO> deleteUser(string userName);

        Task<ResponseListDTO> updateUser(CreateUserDTO createUserDTO);
    }
}
