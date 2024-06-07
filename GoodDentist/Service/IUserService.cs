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
        bool verifyPassword(string inputPassword, string hashedPassword);

        Task<ResponseListDTO> createUser(CreateUserDTO createUserDTO);

        Task<ResponseDTO> getAllUsers(int pageNumber, int rowsPerPage);

        Task<ResponseDTO> deleteUser(string userName);

        Task<ResponseListDTO> updateUser(CreateUserDTO createUserDTO);
    }
}
