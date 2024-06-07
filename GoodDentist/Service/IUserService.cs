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
        byte[] hashPassword(string password, byte[] salt);

        bool verifyPassword(string inputPassword, string hashedPassword);

        Task<ResponseCreateUserDTO> createUser(CreateUserDTO createUserDTO);

        Task<ResponseDTO> getAllUsers(int pageNumber, int rowsPerPage);

        Task<ResponseDTO> deleteUser(string userName);

        Task<ResponseCreateUserDTO> updateUser(CreateUserDTO createUserDTO);
    }
}
