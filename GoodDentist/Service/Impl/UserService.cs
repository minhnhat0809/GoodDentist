
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Http;
using System.Collections.ObjectModel;

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
        private readonly IDistributedCache distributedCache;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IDistributedCache distributedCache, IFirebaseStorageService firebaseStorageService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.distributedCache = distributedCache;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<ResponseListDTO> createUser(CreateUserDTO createUserDTO)
        {
			ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.StatusCode = 200;

            try
            {
                responseDTO = await validateUser(createUserDTO, mod);


                User? user = unitOfWork.userRepo.getUser(createUserDTO.UserName);
                if (user != null)
                {
                    responseDTO.Message.Add("Username is already existed!");
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
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
                user.Avatar = null;

                ClinicUser clinicUser = new ClinicUser()
                {
                    ClinicId = Guid.Parse(createUserDTO.ClinicId),
                    UserId = user.UserId,
                    Status = true
                };

                await unitOfWork.userRepo.CreateAsync(user);
                await unitOfWork.clinicUserRepo.CreateAsync(clinicUser);

                var userDTO = await UploadFile(createUserDTO.Avatar, user.UserId);

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                responseDTO.Result = userDTO;
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
            responseDTO.StatusCode = 200;

            void AddError(string message)
            {
                responseDTO.Message.Add(message);
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 400;
            }

            if (mod)
            {
                //check user name
                if (createUserDTO.UserName.IsNullOrEmpty())
                {
                    AddError("User name cannot be empty!");
                }
                else if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
                {
                    AddError("Username cannot contain special characters!");
                }
                else  if (unitOfWork.userRepo.checkUniqueUserName(createUserDTO.UserName))
                {
                        AddError("User Name is existed!");
                }

                //check password
                var validatePwd = validatePassword(createUserDTO.Password);
                if (validatePwd.Any())
                {
                    responseDTO.Message.AddRange(validatePwd);
                    responseDTO.IsSuccess = false;
                }

                //check name
                if (createUserDTO.Name.IsNullOrEmpty())
                {
                    AddError("Name cannot be empty!");
                }
                else if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
                {
                    AddError("Name cannot contain special characters!");
                }

                //check date of birth
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

                //check gender
                if (createUserDTO.Gender.IsNullOrEmpty())
                {
                    AddError("Gender is empty!");
                }
                else if (!createUserDTO.Gender.Equals("Nam",StringComparison.OrdinalIgnoreCase) && !createUserDTO.Gender.Equals("Nữ", StringComparison.OrdinalIgnoreCase)
                    && !createUserDTO.Gender.Equals("Khác", StringComparison.OrdinalIgnoreCase))
                {
                    AddError("Invalid gender!");
                }

                //check phone number
                if (createUserDTO.PhoneNumber.IsNullOrEmpty())
                {
                    AddError("Phone number is empty!");
                }
                else if (!Regex.IsMatch(createUserDTO.PhoneNumber, @"^\d{10}$"))
                {
                        AddError("Phone number must contain exactly 10 digits");
                    
                }

                //check email
                if (createUserDTO.Email.IsNullOrEmpty())
                {
                    AddError("Email is empty!");
                }
                else if (unitOfWork.userRepo.checkUniqueEmail(createUserDTO.Email))
                {
                    AddError("Email is existed!");
                }

                //check address
                if (createUserDTO.Address.IsNullOrEmpty())
                {
                    AddError("Address is empty!");
                }

                if (createUserDTO.Avatar == null)
                {
                    AddError("Avatar is empty!");
                }

            }
            else
            {
                //check user name
                if (createUserDTO.UserName.IsNullOrEmpty())
                {
                    AddError("User name cannot be empty!");
                }

                //check name
                if (!createUserDTO.Name.IsNullOrEmpty())
                {
                    if (Regex.IsMatch(createUserDTO.UserName, @"[^a-zA-Z0-9]"))
                    {
                        AddError("Name cannot contain special characters!");
                    }
                }

                //check date of birth
                if (createUserDTO.Dob.HasValue)
                {
                    DateTime minDateOfBirth = new DateTime(1900, 1, 1);
                    DateTime maxDateOfBirth = DateTime.Today;
                    if (createUserDTO.Dob < minDateOfBirth || createUserDTO.Dob > maxDateOfBirth)
                    {
                        AddError("Date of birth is outside the reasonable range");
                    }
                }

                //check gender
                if (!createUserDTO.Gender.IsNullOrEmpty())
                {
                    if (!createUserDTO.Gender.Equals("Nam", StringComparison.OrdinalIgnoreCase) && !createUserDTO.Gender.Equals("Nữ", StringComparison.OrdinalIgnoreCase)
                    && !createUserDTO.Gender.Equals("Khác", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError("Invalid gender!");
                    }
                }

                //check phone number
                if (!createUserDTO.PhoneNumber.IsNullOrEmpty())
                {
                    if (!Regex.IsMatch(createUserDTO.PhoneNumber, @"^\d{10}$"))
                    {
                        AddError("Phone number must contain exactly 10 digits");

                    }
                }

                //check email
                if (!createUserDTO.Email.IsNullOrEmpty())
                {
                    User user = unitOfWork.userRepo.getUser(createUserDTO.UserName);
                    if (!user.Email.Equals(createUserDTO.Email))
                    {
                        if (unitOfWork.userRepo.checkUniqueEmail(createUserDTO.Email))
                        {
                            AddError("Email is existed!");
                        }
                    }                   
                }
            }


            //status
            if (createUserDTO.Status == null)
            {
                AddError("Status is empty!");
            }

            //check clinic
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

            //check role
            if (createUserDTO.RoleId <= 0)
            {
                AddError("Role is empty!");
            }
            else
            {
                BusinessObject.Entity.Role? userRole = await unitOfWork.roleRepo.GetRole(createUserDTO.RoleId);
                if (userRole == null)
                {
                    AddError("This userRole does not exist!");
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
            List<User> userList = await unitOfWork.userRepo.GetAllUsers(pageNumber, rowsPerPage);
            try
            {
                userList = FilterUsers(userList, filterField, filterValue);
                userList = SortUsers(userList, sortField, sortOrder);
               
                List<UserDTO> users = mapper.Map<List<UserDTO>>(userList);
                foreach (var user in users)
                {
                    var clinics = userList
                        .FirstOrDefault(x => x.UserId == user.UserId)?
                        .ClinicUsers
                        .Where(cu => cu.Status==true) 
                        .Select(cu => cu.Clinic)
                        .ToList();
                    user.Clinics = mapper.Map<List<ClinicDTO>>(clinics);
                }
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
            responseDTO.StatusCode = 200;
            responseDTO.IsSuccess = true;
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
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                //check name
                if (!createUserDTO.Name.IsNullOrEmpty())
                {
                    user.Name = createUserDTO.Name;
                }

                //check date of birth
                if (createUserDTO.Dob.HasValue)
                {
                    user.Dob = DateOnly.FromDateTime(createUserDTO.Dob.Value);
                }

                //check gender
                if (!createUserDTO.Gender.IsNullOrEmpty())
                {
                    user.Gender = createUserDTO.Gender;
                }

                //check phone number
                if (!createUserDTO.PhoneNumber.IsNullOrEmpty())
                {
                    user.PhoneNumber = createUserDTO.PhoneNumber;
                }

                //check email
                if (!createUserDTO.Email.IsNullOrEmpty())
                {
                    user.Email = createUserDTO.Email;
                }

                if (!createUserDTO.Address.IsNullOrEmpty())
                {
                    user.Address = createUserDTO.Address;
                }
                
                user.RoleId = createUserDTO.RoleId;

                if (createUserDTO.Reset == true)
                {
                    user.Salt = salting();
                    user.Password = hashPassword("12345678.C", user.Salt);
                }

                if (user.Status == false && createUserDTO.Status == true)
                {
                    ClinicUser? clinicUser = await unitOfWork.clinicUserRepo.GetClinicUserByUserAndClinicc(user.UserId.ToString(), createUserDTO.ClinicId);

                    if (clinicUser == null)
                    {
                        clinicUser = new ClinicUser()
                        {
                            ClinicId = Guid.Parse(createUserDTO.ClinicId),
                            UserId = user.UserId,
                            Status = true
                        };

                        await unitOfWork.clinicUserRepo.CreateAsync(clinicUser);
                    }
                    else
                    {
                        clinicUser.Status = false;

                        await unitOfWork.clinicUserRepo.UpdateAsync(clinicUser);
                    }
                }
                else
                {
                    ClinicUser? clinicUserOld = await unitOfWork.clinicUserRepo.GetClinicUserByUserAndClinicNow(user.UserId.ToString());

                    if (clinicUserOld == null)
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message.Add("User is not belong to any clinics!");
                        responseDTO.StatusCode = 400;
                        return responseDTO;
                    }
                    if (createUserDTO.Status == false)
                    {
                        clinicUserOld.Status = false;
                        await unitOfWork.clinicUserRepo.UpdateAsync(clinicUserOld);
                    }
                    else if (!clinicUserOld.ClinicId.ToString().Equals(createUserDTO.ClinicId, StringComparison.OrdinalIgnoreCase))
                    {
                        ClinicUser? clinicUserNew = await unitOfWork.clinicUserRepo.GetClinicUserByUserAndClinicc(clinicUserOld.UserId.ToString(), createUserDTO.ClinicId);
                        if (clinicUserNew == null)
                        {
                            clinicUserNew = new ClinicUser()
                            {
                                ClinicId = Guid.Parse(createUserDTO.ClinicId),
                                UserId = user.UserId,
                                Status = true
                            };
                            clinicUserOld.Status = false;

                            await unitOfWork.clinicUserRepo.UpdateAsync(clinicUserOld);

                            await unitOfWork.clinicUserRepo.CreateAsync(clinicUserNew);


                        }
                        else
                        {
                            clinicUserNew.Status = true;
                            clinicUserOld.Status = false;

                            await unitOfWork.clinicUserRepo.UpdateAsync(clinicUserNew);

                            await unitOfWork.clinicUserRepo.UpdateAsync(clinicUserOld);

                        }
                    }
                }

                user.Status = createUserDTO.Status;
                await unitOfWork.userRepo.UpdateAsync(user);

                UserDTO userDTO = mapper.Map<UserDTO>(user);
                if (createUserDTO.Avatar != null)
                {
                     userDTO = await UploadFile(createUserDTO.Avatar, user.UserId);
                }

                responseDTO.Message.Add("Update sucessfully");
                responseDTO.IsSuccess = true;
                responseDTO.Result = userDTO;
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
                    case "search":
                        return users = users.Where(x =>
                            x.UserName.ToLower().Contains(filterValue) ||
                            x.Name.ToLower().Contains(filterValue) ||
                            (x.PhoneNumber != null && x.PhoneNumber.Contains(filterValue)) ||
                            (x.Email != null && x.Email.Contains(filterValue))
                        ).ToList();
                    case "clinic" :
                    if (filterValue.Equals("all",StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                        return users = users
                            .Where(user => user.ClinicUsers.Any(cu => cu.ClinicId.ToString() == filterValue && cu.Status == true))
                            .ToList();
                    default:
                        return users;
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
                case "name":
                    return isAscending ? users.OrderBy(u => u.Name).ToList() : users.OrderByDescending(u => u.Name).ToList();
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
        public async Task<UserDTO> UploadFile(IFormFile file, Guid userId)
        {
            var model = await unitOfWork.userRepo.GetByIdAsync(userId);
            if (model == null)
            {
                throw new Exception("User not found!");
            }
            if (model.Avatar != null)
            {
                // Delete the image before add new one
                await _firebaseStorageService.DeleteFileAndReference(model.Avatar);
            }

            // Generate a unique file name
            var fileName = $"{model.UserId}";
        
            // Upload image to Firebase Storage
            var urlAvatar = await _firebaseStorageService.UploadFile(fileName, file, "user");

            // Update the URL in the medical record model
            model.Avatar = urlAvatar;
            if (await unitOfWork.userRepo.UpdateAsync(model))
            {
                model = await unitOfWork.userRepo.GetByIdAsync(model.UserId);
            }
        
            return mapper.Map<UserDTO>(model);
        }

        public async Task<UserDTO> DeleteFile(Guid userId)
        {
            var model = await unitOfWork.userRepo.GetByIdAsync(userId);
            if (model == null)
            {
                throw new Exception("User not found!");
            }
            // Delete image to Firebase Storage
            await _firebaseStorageService.DeleteFileAndReference(model.Avatar);

            // Update the URL in the medical record model;
            model.Avatar = null;
            if (await unitOfWork.userRepo.UpdateAsync(model))
            {
                model = await unitOfWork.userRepo.GetByIdAsync(model.UserId);
            }
        
            return mapper.Map<UserDTO>(model);
        }

        public async Task<ResponseDTO> GetUser(Guid userId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                if (userId == null)
                {
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "User name is empty!";
                    return responseDTO;
                }

                User? user = await unitOfWork.userRepo.GetByIdAsync(userId);
                if (user == null)
                {
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "There are no users found !";
                    return responseDTO;
                }
                UserDTO? dto = mapper.Map<UserDTO>(user);
                List<Clinic> userClinics = await unitOfWork.clinicRepo.GetClinicByUserId(userId);
                dto.Clinics = mapper.Map<List<ClinicDTO>>(userClinics);
                responseDTO.Result = dto;
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
    }
}
