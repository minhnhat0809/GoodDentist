
using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Services.Impl
{
    public class AccountService : IAccountService
    {
        private const int saltSize = 128 / 8;
        private const int keySize = 256 / 8;
        private const int iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        private readonly IMapper mapper;
        private readonly IAccountRepo accountRepo;
        
        public AccountService(IMapper mapper, IAccountRepo accountRepo)
        {
            this.mapper = mapper;
            this.accountRepo = accountRepo;
        }

        public async Task<ResponseCreateUserDTO> createUser(CreateUserDTO createUserDTO)
        {
            ResponseCreateUserDTO responseDTO = new ResponseCreateUserDTO();
            try
            {
               responseDTO = validateUser(createUserDTO);

                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }

                var user = mapper.Map<User>(createUserDTO);
                user.Salt = salting();
                user.Password = hashPassword(createUserDTO.Password, user.Salt);
                user.UserId = Guid.NewGuid();

                accountRepo.CreateUser(user);

                responseDTO.Message.Add("Create sucessfully");  
                responseDTO.IsSuccess = true;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Result = null;
                responseDTO.Message.Add(ex.Message);
                responseDTO.IsSuccess = false;
                return responseDTO;
            }           
        }

        public bool verifyPassword(string inputPassword, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        private List<string> validatePassword(string password)
        {
            List<string> result = new List<string>();

            if (password.Length < 8)
            {
                result.Add("Minimum length of Password is 8!");
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$"))
            {
                result.Add("Password must has at least one lowercase letter, one uppercase letter, one number and one special character!");
            }

            return result;
        }

        private ResponseCreateUserDTO validateUser(CreateUserDTO createUserDTO)
        {
            ResponseCreateUserDTO responseDTO = new ResponseCreateUserDTO();
            responseDTO.IsSuccess = true;

            if (createUserDTO.UserName.IsNullOrEmpty())
            {
                responseDTO.Message.Add("User name cannot be empty!");
                responseDTO.IsSuccess = false;
            }
            else
            {
                if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
                {
                    responseDTO.Message.Add("Username cannot contain special characters!");
                    responseDTO.IsSuccess = false;
                }
                else 
                {
                    if (accountRepo.checkExistUser(createUserDTO.UserName))
                    {
                        responseDTO.Message.Add("Username is already existed!");
                        responseDTO.IsSuccess = false;
                    }
                }
            }

            var validatePwd = validatePassword(createUserDTO.Password);

            if (validatePwd.Count != 0)
            {
                responseDTO.Message.AddRange(validatePwd);
                responseDTO.IsSuccess = false;
            }

            if (!createUserDTO.Dob.HasValue)
            {
                responseDTO.Message.Add("Date of birth is empty!");
                responseDTO.IsSuccess = false;
            }
            else
            {
                DateTime minDateOfBirth = new DateTime(1900, 1, 1);
                DateTime maxDateOfBirth = DateTime.Today;
                if (createUserDTO.Dob < minDateOfBirth || createUserDTO.Dob > maxDateOfBirth)
                {
                    responseDTO.Message.Add("Date of birth is outside the reasonable range");
                    responseDTO.IsSuccess = false;
                }
            }

            if (createUserDTO.Gender.IsNullOrEmpty())
            {
                responseDTO.Message.Add("Gender is empty!");
                responseDTO.IsSuccess = false;
            }

            if (createUserDTO.PhoneNumber.IsNullOrEmpty())
            {
                responseDTO.Message.Add("Phone number is empty!");
                responseDTO.IsSuccess = false;
            }
            else
            {
                if (!Regex.IsMatch(createUserDTO.PhoneNumber, @"^\d{10}$"))
                {
                    responseDTO.Message.Add("Phone number only contains digits and 10 digits");
                    responseDTO.IsSuccess = false;
                }
            }

            if (createUserDTO.Email.IsNullOrEmpty())
            {
                responseDTO.Message.Add("Email is empty!");
                responseDTO.IsSuccess = false;
            }

            if (createUserDTO.Address.IsNullOrEmpty())
            {
                responseDTO.Message.Add("Address is empty!");
                responseDTO.IsSuccess = false;
            }

            if (createUserDTO.Status == null)
            {
                responseDTO.Message.Add("Status is empty!");
                responseDTO.IsSuccess = false;
            }

            if (createUserDTO.RoleId == 0)
            {
                responseDTO.Message.Add("Role is empty!");
                responseDTO.IsSuccess = false;
            }

            return responseDTO;
        }

        public byte[] hashPassword(string password, byte[] salt)
        {
            var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            var pwdString = string.Join(Convert.ToBase64String(salt), Convert.ToBase64String(hashedPassword));
            return Convert.FromBase64String(pwdString);
        }

        private byte[] salting()
        {
            return RandomNumberGenerator.GetBytes(saltSize);
        }

        public async Task<ResponseDTO> getAllUsers()
        {
            try
            {
                List<User> userList = await accountRepo.GetAllUsers();

                List<UserDTO> users = mapper.Map<List<UserDTO>>(userList);
                
                return new ResponseDTO("Get users successfully!", 200, true, users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }
    }
}
