
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

                if (responseDTO.isSuccess == false)
                {
                    return responseDTO;
                }

                var user = mapper.Map<User>(createUserDTO);

                user.UserId = Guid.NewGuid();

                accountRepo.CreateUser(user);

                responseDTO.message.Add("Create sucessfully");  
                responseDTO.isSuccess = true;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.result = null;
                responseDTO.message.Add(ex.Message);
                responseDTO.isSuccess = false;
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
            responseDTO.isSuccess = true;

            if (createUserDTO.UserName.IsNullOrEmpty())
            {
                responseDTO.message.Add("User name cannot be empty!");
                responseDTO.isSuccess = false;
            }
            else
            {
                if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
                {
                    responseDTO.message.Add("Username cannot contain special characters!");
                    responseDTO.isSuccess = false;
                }
                else 
                {
                    if (accountRepo.checkExistUser(createUserDTO.UserName))
                    {
                        responseDTO.message.Add("Username is already existed!");
                        responseDTO.isSuccess = false;
                    }
                }
            }

            var validatePwd = validatePassword(createUserDTO.Password);

            if (validatePwd.Count != 0)
            {
                responseDTO.message.AddRange(validatePwd);
                responseDTO.isSuccess = false;
            }

            if (!createUserDTO.Dob.HasValue)
            {
                responseDTO.message.Add("Date of birth is empty!");
                responseDTO.isSuccess = false;
            }
            else
            {
                DateTime minDateOfBirth = new DateTime(1900, 1, 1);
                DateTime maxDateOfBirth = DateTime.Today;
                if (createUserDTO.Dob < minDateOfBirth || createUserDTO.Dob > maxDateOfBirth)
                {
                    responseDTO.message.Add("Date of birth is outside the reasonable range");
                    responseDTO.isSuccess = false;
                }
            }

            if (createUserDTO.Gender.IsNullOrEmpty())
            {
                responseDTO.message.Add("Gender is empty!");
                responseDTO.isSuccess = false;
            }

            if (createUserDTO.PhoneNumber.IsNullOrEmpty())
            {
                responseDTO.message.Add("Phone number is empty!");
                responseDTO.isSuccess = false;
            }
            else
            {
                if (!Regex.IsMatch(createUserDTO.PhoneNumber, @"^\d{10}$"))
                {
                    responseDTO.message.Add("Phone number only contains digits and 10 digits");
                    responseDTO.isSuccess = false;
                }
            }

            if (createUserDTO.Email.IsNullOrEmpty())
            {
                responseDTO.message.Add("Email is empty!");
                responseDTO.isSuccess = false;
            }

            if (createUserDTO.Address.IsNullOrEmpty())
            {
                responseDTO.message.Add("Address is empty!");
                responseDTO.isSuccess = false;
            }

            if (createUserDTO.Status == null)
            {
                responseDTO.message.Add("Status is empty!");
                responseDTO.isSuccess = false;
            }

            if (createUserDTO.RoleId == 0)
            {
                responseDTO.message.Add("Role is empty!");
                responseDTO.isSuccess = false;
            }

            return responseDTO;
        }

        public byte[] hashPassword(string password)
        {
            var salt = salting();
            var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            var pwdString = string.Join(Convert.ToBase64String(salt), Convert.ToBase64String(hashedPassword));
            return Convert.FromBase64String(pwdString);
        }

        private byte[] salting()
        {
            return RandomNumberGenerator.GetBytes(saltSize);
        }
    }
}
