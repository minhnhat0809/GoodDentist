using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountService
    {
        byte[] hashPassword(string password, byte[] salt);

        bool verifyPassword(string inputPassword, string hashedPassword);

        Task<ResponseCreateUserDTO> createUser(CreateUserDTO createUserDTO);

        Task<ResponseDTO> getAllUsers();

        Task<ResponseDTO> deleteUser(string userName);
    }
}
