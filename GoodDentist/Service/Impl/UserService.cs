
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Services.Impl
{
    public class UserService : IUserService
    {
        private const int saltSize = 128 / 8;
        private const int keySize = 256 / 8;
        private const int iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        private bool mod = true;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResponseListDTO> createUser(CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = new ResponseListDTO();
            try
            {
                responseDTO = await validateUser(createUserDTO, mod);


                User? user = unitOfWork.userRepo.getUser(createUserDTO.UserName);
                if (user != null)
                {
                    responseDTO.Message.Add("Username is already existed!");
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }

                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }

                user = mapper.Map<User>(createUserDTO);
                user.Salt = salting();
                user.Password = hashPassword(createUserDTO.Password, user.Salt);
                user.UserId = Guid.NewGuid();
                user.CreatedDate = DateTime.Now;

                ClinicUser clinicUser = new ClinicUser()
                {
                    ClinicId = Guid.Parse(createUserDTO.ClinicId),
                    UserId = user.UserId,
                    Status = true
                };

                await unitOfWork.userRepo.CreateAsync(user);
                await unitOfWork.clinicUserRepo.CreateAsync(clinicUser);

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

        private async Task<ResponseListDTO> validateUser(CreateUserDTO createUserDTO, bool mod)
        {
			ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.IsSuccess = true;

            void AddError(string message)
            {
                responseDTO.Message.Add(message);
                responseDTO.IsSuccess = false;
            }

            if (createUserDTO.UserName.IsNullOrEmpty())
            {
                AddError("User name cannot be empty!");
            }
            else if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
            {
                AddError("Username cannot contain special characters!");
            }

            if (mod)
            {
                var validatePwd = validatePassword(createUserDTO.Password);

                if (validatePwd.Any())
                {
                    responseDTO.Message.AddRange(validatePwd);
                    responseDTO.IsSuccess = false;
                }
            }

            if (createUserDTO.Name.IsNullOrEmpty())
            {
                AddError("Name cannot be empty!");
            }
            else if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
            {
                AddError("Name cannot contain special characters!");
            }

            if (!createUserDTO.Dob.HasValue)
            {
                AddError("Date of birth is empty!");
            }
            else
            {
                DateTime minDateOfBirth = new DateTime(1900, 1, 1);
                DateTime maxDateOfBirth = DateTime.Today;
                if (createUserDTO.Dob < minDateOfBirth || createUserDTO.Dob > maxDateOfBirth)
                {
                    AddError("Date of birth is outside the reasonable range");
                }
            }

            if (createUserDTO.Gender.IsNullOrEmpty())
            {
                AddError("Gender is empty!");
            }

            if (createUserDTO.PhoneNumber.IsNullOrEmpty())
            {
                AddError("Phone number is empty!");
            }
            else
            {
                if (!Regex.IsMatch(createUserDTO.PhoneNumber, @"^\d{10}$"))
                {
                    AddError("Phone number must contain exactly 10 digits");
                }
            }

            if (createUserDTO.Email.IsNullOrEmpty())
            {
                AddError("Email is empty!");
            }

            if (createUserDTO.Address.IsNullOrEmpty())
            {
                AddError("Address is empty!");
            }

            if (createUserDTO.Status == null)
            {
                AddError("Status is empty!");
            }

            if (createUserDTO.ClinicId.IsNullOrEmpty())
            {
                AddError("Please choose a clinic!");
            }
            else
            {
                Clinic? clinic = await unitOfWork.clinicRepo.getClinicById(createUserDTO.ClinicId);
                if (clinic == null)
                {
                    AddError("Clinic does not exist!");
                }
            }

            if (createUserDTO.RoleId == 0)
            {
                AddError("Role is empty!");
            }
            else
            {
                Role? role = await unitOfWork.roleRepo.GetRole(createUserDTO.RoleId);
                if (role == null)
                {
                    AddError("This role does not exist!");
                }
            }

            return responseDTO;
        }

        private byte[] hashPassword(string password, byte[] salt)
        {
            var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            var pwdString = string.Join(Convert.ToBase64String(salt), Convert.ToBase64String(hashedPassword));
            return Convert.FromBase64String(pwdString);
        }

        private byte[] salting()
        {
            return RandomNumberGenerator.GetBytes(saltSize);
        }

        public async Task<ResponseDTO> getAllUsers(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField, string? sortOrder)
        {
            try
            {
                List<User> userList = await unitOfWork.userRepo.GetAllUsers(pageNumber, rowsPerPage);

                userList = FilterUsers(userList, filterField, filterValue);
                userList = SortUsers(userList, sortField, sortOrder);

                List<UserDTO> users = mapper.Map<List<UserDTO>>(userList);

                return new ResponseDTO("Get users successfully!", 200, true, users);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        public async Task<ResponseDTO> deleteUser(string userName)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                if (userName.IsNullOrEmpty())
                {
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "User name is empty!";
                    return responseDTO;
                }

                User? user = unitOfWork.userRepo.getUser(userName);
                if (user == null)
                {
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "There are no users with this user name!";
                    return responseDTO;
                }

                user.Status = false;
                await unitOfWork.userRepo.DeleteAsync(user);
                responseDTO.Message = "Delete successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message = ex.Message;
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                return responseDTO;
            }

        }

        public async Task<ResponseListDTO> updateUser(CreateUserDTO createUserDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            mod = false;
            try
            {
                responseDTO = await validateUser(createUserDTO, mod);

                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }

                var user = unitOfWork.userRepo.getUser(createUserDTO.UserName);
                if (user == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message.Add("User is not existed!");
                    return responseDTO;
                }

                var userId = user.UserId;
                ClinicUser? clinicUserOld = await unitOfWork.clinicUserRepo.GetClinicUserByUserAndClinicNow(userId.ToString());
                user = mapper.Map<User>(createUserDTO);
                user.UserId = userId;

                if (clinicUserOld == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message.Add("User is not belong to any clinics!");
                    return responseDTO;
                }

                if (!clinicUserOld.ClinicId.Equals(createUserDTO.ClinicId))
                {
                    ClinicUser? clinicUserNew = await unitOfWork.clinicUserRepo.GetClinicUserByUserAndClinic(clinicUserOld.UserId.ToString(), createUserDTO.ClinicId);
                    if (clinicUserNew == null)
                    {
                        clinicUserNew = new ClinicUser()
                        {
                            ClinicId = Guid.Parse(createUserDTO.ClinicId),
                            UserId = user.UserId,
                            Status = true
                        };
                        clinicUserOld.Status = false;

                        unitOfWork.clinicUserRepo.CreateAsync(clinicUserNew);
                        unitOfWork.clinicUserRepo.UpdateAsync(clinicUserOld);
                    }
                    else
                    {
                        clinicUserNew.Status = true;
                        clinicUserOld.Status = false;

                        unitOfWork.clinicUserRepo.UpdateAsync(clinicUserNew);
                        unitOfWork.clinicUserRepo.UpdateAsync(clinicUserOld);
                    }
                }

                unitOfWork.userRepo.UpdateAsync(user);
                responseDTO.Message.Add("Update sucessfully");
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

        private List<User> FilterUsers(List<User> users, string filterField, string filterValue)
        {
            if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
            {
                return users;
            }

            switch (filterField.ToLower())
            {
                case "username":
                    return users.Where(u => u.UserName.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "dob":
                    if (DateOnly.TryParse(filterValue, out var dob))
                    {
                        return users.Where(u => u.Dob == dob).ToList();
                    }
                    break;
                case "gender":
                    return users.Where(u => u.Gender != null && u.Gender.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "phonenumber":
                    return users.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "email":
                    return users.Where(u => u.Email != null && u.Email.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "address":
                    return users.Where(u => u.Address != null && u.Address.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "roleid":
                    if (int.TryParse(filterValue, out var roleId))
                    {
                        return users.Where(u => u.RoleId == roleId).ToList();
                    }
                    break;
                case "status":
                    if (bool.TryParse(filterValue, out var status))
                    {
                        return users.Where(u => u.Status == status).ToList();
                    }
                    break;
            }
            return users;
        }

        private List<User> SortUsers(List<User> users, string sortField, string sortOrder)
            {
            if (string.IsNullOrEmpty(sortField) || string.IsNullOrEmpty(sortOrder))
            {
                return users;
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            switch (sortField.ToLower())
            {
                case "username":
                    return isAscending ? users.OrderBy(u => u.UserName).ToList() : users.OrderByDescending(u => u.UserName).ToList();
                case "dob":
                    return isAscending ? users.OrderBy(u => u.Dob).ToList() : users.OrderByDescending(u => u.Dob).ToList();
                case "gender":
                    return isAscending ? users.OrderBy(u => u.Gender).ToList() : users.OrderByDescending(u => u.Gender).ToList();
                case "phonenumber":
                    return isAscending ? users.OrderBy(u => u.PhoneNumber).ToList() : users.OrderByDescending(u => u.PhoneNumber).ToList();
                case "email":
                    return isAscending ? users.OrderBy(u => u.Email).ToList() : users.OrderByDescending(u => u.Email).ToList();
                case "address":
                    return isAscending ? users.OrderBy(u => u.Address).ToList() : users.OrderByDescending(u => u.Address).ToList();
                case "roleid":
                    return isAscending ? users.OrderBy(u => u.RoleId).ToList() : users.OrderByDescending(u => u.RoleId).ToList();
                case "status":
                    return isAscending ? users.OrderBy(u => u.Status).ToList() : users.OrderByDescending(u => u.Status).ToList();                    
            }

            return users;
        }

        public async Task<ResponseDTO> getAllUsersByClinic(string clinicId, int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField, string? sortOrder)
        {
            try
            {
                List<User> userList = await unitOfWork.clinicUserRepo.GetAllUsersByClinic(clinicId, pageNumber, rowsPerPage);

                userList = FilterUsers(userList, filterField, filterValue);
                userList = SortUsers(userList, sortField, sortOrder);

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
