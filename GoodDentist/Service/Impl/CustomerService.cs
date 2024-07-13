using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private const int saltSize = 128 / 8;
        private const int keySize = 256 / 8;
        private const int iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        private bool mod = true;
        
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFirebaseStorageService firebaseStorageService;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId, string search)
        {
            ResponseDTO responseDTO = new ResponseDTO(" Get All Customer By Dentist Successfully", 200, true, null);
            try
            {
                if (dentistId.IsNullOrEmpty())
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Dentist ID is null!";
                    return responseDTO;
                }

                User? dentist = await unitOfWork.userRepo.GetByIdAsync(Guid.Parse(dentistId));
                if (dentist == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Dentist is not exist!";
                    return responseDTO;
                }

                List<ExaminationProfile> examinationProfiles = await unitOfWork.examProfileRepo.GetProfileByDenitst(dentistId);

                List<Customer> customers = examinationProfiles.Select(e => e.Customer).ToList();

                if (!search.IsNullOrEmpty())
                {
                    string pattern = $@"\b{Regex.Escape(search)}\b";
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    customers = customers.Where(c => regex.IsMatch(c.Name) || c.PhoneNumber.Contains(search) ||
                    c.Email.Contains(search)).ToList();
                }

                List<UserDTO> customerDTO = new List<UserDTO>();
                foreach (var u in customers)
                {
                    List<Clinic> clinics = u.CustomerClinics.Select(cc => cc.Clinic).ToList();
                    UserDTO userDTO = mapper.Map<UserDTO>(u);
                    userDTO.Clinics = mapper.Map<List<ClinicDTO>>(clinics);
                    customerDTO.Add(userDTO);
                }

                responseDTO.Result = customerDTO;
                responseDTO.Message = "Get successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> GetAllCustomers(string search, int pageNumber, int rowsPerPage)
        {
            ResponseDTO responseDTO = new ResponseDTO("Get All Customer Successfully", 200, true, null);
            try
            {
                List<Customer> customers = await unitOfWork.customerRepo.GetAllCustomers(pageNumber, rowsPerPage);
                if (!search.IsNullOrEmpty())
                {
                    string pattern = $@"\b{Regex.Escape(search)}\b";
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    customers = customers.Where(c => regex.IsMatch(c.Name) || c.PhoneNumber.Contains(search)).ToList();
                }

                List<UserDTO> userDTOs = new List<UserDTO>();
                foreach (var u in customers)
                {
                    List<Clinic> clinics = u.CustomerClinics.Select(cc => cc.Clinic).ToList();
                    UserDTO userDTO = mapper.Map<UserDTO>(u);
                    userDTO.Clinics = mapper.Map<List<ClinicDTO>>(clinics);
                    userDTOs.Add(userDTO);
                }

                responseDTO.Result = userDTOs;
            }
            catch (Exception e)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = e.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetCustomerById(string customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("Get Customer Successfully", 200, true, null);
            try
            {
                if (customerId.IsNullOrEmpty())
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Customer ID is null!";
                    return responseDTO;
                }

                Customer customer = await unitOfWork.customerRepo.GetCustomerById(Guid.Parse(customerId));
                if (customer == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 404;
                    responseDTO.Message = "Customer not found!";
                    return responseDTO;
                }

                UserDTO customerDTO = mapper.Map<UserDTO>(customer);
                customerDTO.Clinics = mapper.Map<List<ClinicDTO>>(customer.CustomerClinics.Select(x=>x.Clinic).ToList());
                responseDTO.Result = customerDTO;
                responseDTO.Message = "Get successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> DeleteCustomer(Guid customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("Delete Customer Successfully", 200, true, null);
            try
            {
                Customer model = await unitOfWork.customerRepo.GetCustomerById(customerId);
                if (model == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "There no customer founded yet!";
                    responseDTO.Result = null;
                }
                await unitOfWork.customerRepo.DeleteCustomer(customerId);
                responseDTO.Message = "Customer delete successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }
        
        public async Task<ResponseDTO> CreateCustomer(CustomerRequestDTO customerDto)
        {
            ResponseDTO responseDTO = new ResponseDTO("Create Customer Successfully", 200, true, null);
            try
            {
                if (customerDto == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Customer data is null!";
                    return responseDTO;
                }

                // Validate Customer DTO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(customerDto);
                if (!Validator.TryValidateObject(customerDto, validationContext, validationResults, true))
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return responseDTO;
                }

                // Ensure ClinicId is a valid GUID
                if (!Guid.TryParse(customerDto.ClinicId, out Guid clinicGuid))
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Invalid Clinic ID format!";
                    return responseDTO;
                }

                Clinic clinic = await unitOfWork.clinicRepo.getClinicById(customerDto.ClinicId);
                if (clinic == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Clinic is null!";
                    return responseDTO;
                }
                
                Customer customer = mapper.Map<Customer>(customerDto);
                customer.CustomerId = Guid.NewGuid();
                customer.Salt = salting();
                customer.Password = hashPassword(customerDto.Password, customer.Salt);
                customer.CreatedDate = DateTime.Now;
                customer.Avatar = null;
                customer.Status = true;
                
                CustomerClinic customerClinic = new CustomerClinic()
                {
                    ClinicId = clinicGuid,
                    Status = true,
                    CustomerId = customer.CustomerId,
                    Customer = customer,
                    Clinic = clinic
                };
                
                customer.CustomerClinics.Add(customerClinic);
                
                ExaminationProfile profile = new ExaminationProfile()
                {
                    Customer = customer,
                    CustomerId = customer.CustomerId, 
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Status = true
                };
                customer.ExaminationProfiles.Add(profile);

                await unitOfWork.customerRepo.CreateCustomer(customer);

                responseDTO.Result = customerDto;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> UpdateCustomer(CustomerRequestDTO customerDto)
        {
            ResponseDTO responseDTO = new ResponseDTO("Update Customer Successfully", 200, true, null);
            try
            {   
                if (customerDto == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Customer data is null!";
                    return responseDTO;
                }

                Customer customer = await unitOfWork.customerRepo.GetCustomerByPhoneOrEmailOrUsername(customerDto.UserName);
                
                if (customer == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Customer not found!";
                    return responseDTO;
                }
                
                customer = mapper.Map<Customer>(customerDto);
                await unitOfWork.customerRepo.UpdateCustomer(customer);
                responseDTO.Message = "Customer updated successfully!";
                responseDTO.Result = mapper.Map<CustomerDTO>(customer);
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }
        
        public async Task<ResponseListDTO> updateCustomer(CustomerRequestDTO customerRequestDto)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.StatusCode = 200;
            responseDTO.IsSuccess = true;
            mod = false;
            try
            {
                responseDTO = await validateCustomer(customerRequestDto, mod);

                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }

                var model = await unitOfWork.customerRepo.GetCustomerByPhoneOrEmailOrUsername(customerRequestDto.UserName);
                if (model == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message.Add("Customer is not existed!");
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                //check name
                if (!customerRequestDto.Name.IsNullOrEmpty())
                {
                    model.Name = customerRequestDto.Name;
                }

                //check date of birth
                if (customerRequestDto.Dob.HasValue)
                {
                    model.Dob = customerRequestDto.Dob.Value;
                }

                //check gender
                if (!customerRequestDto.Gender.IsNullOrEmpty())
                {
                    model.Gender = customerRequestDto.Gender;
                }

                //check phone number
                if (!customerRequestDto.PhoneNumber.IsNullOrEmpty())
                {
                    model.PhoneNumber = customerRequestDto.PhoneNumber;
                }

                //check email
                if (!customerRequestDto.Email.IsNullOrEmpty())
                {
                    model.Email = customerRequestDto.Email;
                }

                if (!customerRequestDto.Address.IsNullOrEmpty())
                {
                    model.Address = customerRequestDto.Address;
                }
                

                if (customerRequestDto.Reset == true)
                {
                    model.Salt = salting();
                    model.Password = hashPassword("12345678.C", model.Salt);
                }

                if (model.Status == false && customerRequestDto.Status == true)
                {
                    CustomerClinic? customerClinic =
                        await unitOfWork.customerRepo.GetCustomerClinicByCustomerAndClinic(model.CustomerId,
                            Guid.Parse((ReadOnlySpan<char>)customerRequestDto.ClinicId));
                    if (customerClinic == null)
                    {
                        customerClinic = new CustomerClinic()
                        {
                            ClinicId = Guid.Parse(customerRequestDto.ClinicId),
                            CustomerId = model.CustomerId,
                            Status = true
                        };
                        //model.CustomerClinics.Add(customerClinic);
                        await unitOfWork.CustomerClinicRepository.CreateAsync(customerClinic);
                    }
                    else
                    {
                        customerRequestDto.Status = true;
                        await unitOfWork.CustomerClinicRepository.UpdateAsync(customerClinic);
                    }
                }
                else
                {
                    CustomerClinic? clinicCustomerOld = await unitOfWork.CustomerClinicRepository.GetCustomerClinicByCustomerAndClinicNow(model.CustomerId.ToString());

                    if (clinicCustomerOld == null)
                    {
                        responseDTO.IsSuccess = false;
                        responseDTO.Message.Add("Customer is not belong to any clinics!");
                        responseDTO.StatusCode = 400;
                        return responseDTO;
                    }
                    if (clinicCustomerOld.Status == false)
                    {
                        clinicCustomerOld.Status = false;
                        await unitOfWork.CustomerClinicRepository.UpdateAsync(clinicCustomerOld);
                    }
                    else if (clinicCustomerOld.ClinicId == clinicCustomerOld.ClinicId)
                    {
                        CustomerClinic? customerClinicNew = await unitOfWork.CustomerClinicRepository.GetCustomerClinicByCustomerAndClinic(clinicCustomerOld.CustomerId.ToString(), customerRequestDto.ClinicId);
                        if (customerClinicNew == null)
                        {
                            customerClinicNew = new CustomerClinic()
                            {
                                ClinicId = Guid.Parse(customerRequestDto.ClinicId),
                                Customer = model,
                                Status = true
                            };
                            clinicCustomerOld.Status = false;

                            await unitOfWork.CustomerClinicRepository.UpdateAsync(clinicCustomerOld);

                            await unitOfWork.CustomerClinicRepository.CreateAsync(customerClinicNew);


                        }
                        else
                        {
                            customerClinicNew.Status = true;
                            clinicCustomerOld.Status = false;

                            await unitOfWork.CustomerClinicRepository.UpdateAsync(customerClinicNew);

                            await unitOfWork.CustomerClinicRepository.UpdateAsync(clinicCustomerOld);

                        }
                    }
                }

                model.Status = customerRequestDto.Status;
                await unitOfWork.customerRepo.UpdateAsync(model);

                CustomerDTO viewModel = mapper.Map<CustomerDTO>(model);
                if (viewModel.Avatar != null)
                {
                    responseDTO.Result = await UploadFile(customerRequestDto.Avatar, model.CustomerId);
                }

                responseDTO.Message.Add("Update sucessfully");
                responseDTO.IsSuccess = true;
                responseDTO.Result = viewModel;
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
        
        private async Task<ResponseListDTO> validateCustomer(CustomerRequestDTO customerRequestDto, bool mod)
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
                //check cus name
                if (customerRequestDto.UserName.IsNullOrEmpty())
                {
                    AddError("User name cannot be empty!");
                }
                else if (Regex.IsMatch(customerRequestDto.UserName, @"[^a-zA-Z0-9]"))
                {
                    AddError("Username cannot contain special characters!");
                }
                else  if (unitOfWork.userRepo.checkUniqueUserName(customerRequestDto.UserName))
                {
                        AddError("User Name is existed!");
                }

                //check password
                var validatePwd = validatePassword(customerRequestDto.Password);
                if (validatePwd.Any())
                {
                    responseDTO.Message.AddRange(validatePwd);
                    responseDTO.IsSuccess = false;
                }

                //check name
                if (customerRequestDto.Name.IsNullOrEmpty())
                {
                    AddError("Name cannot be empty!");
                }
                else if (Regex.IsMatch(customerRequestDto.UserName, @"[^a-zA-Z0-9]"))
                {
                    AddError("Name cannot contain special characters!");
                }

                //check date of birth
                if (!customerRequestDto.Dob.HasValue)
                {
                    AddError("Date of birth is empty!");
                }
                else
                {
                    DateOnly minDateOfBirth = new DateOnly(1900, 1, 1);
                    DateOnly maxDateOfBirth = DateOnly.FromDateTime(DateTime.Now); // Or a specific end date if required

                    // Use DateOnly directly from customerRequestDto.Dob
                    DateOnly dob = customerRequestDto.Dob.Value;

                    if (dob < minDateOfBirth || dob > maxDateOfBirth)
                    {
                        AddError("Date of birth is outside the reasonable range");
                    }
                }

                //check gender
                if (customerRequestDto.Gender.IsNullOrEmpty())
                {
                    AddError("Gender is empty!");
                }
                else if (!customerRequestDto.Gender.Equals("Nam",StringComparison.OrdinalIgnoreCase) && !customerRequestDto.Gender.Equals("Nữ", StringComparison.OrdinalIgnoreCase)
                    && !customerRequestDto.Gender.Equals("Khác", StringComparison.OrdinalIgnoreCase))
                {
                    AddError("Invalid gender!");
                }

                //check phone number
                if (customerRequestDto.PhoneNumber.IsNullOrEmpty())
                {
                    AddError("Phone number is empty!");
                }
                else if (!Regex.IsMatch(customerRequestDto.PhoneNumber, @"^\d{10}$"))
                {
                        AddError("Phone number must contain exactly 10 digits");
                    
                }

                //check email
                if (customerRequestDto.Email.IsNullOrEmpty())
                {
                    AddError("Email is empty!");
                }
                else if (unitOfWork.userRepo.checkUniqueEmail(customerRequestDto.Email))
                {
                    AddError("Email is existed!");
                }

                //check address
                if (customerRequestDto.Address.IsNullOrEmpty())
                {
                    AddError("Address is empty!");
                }

                if (customerRequestDto.Avatar == null)
                {
                    AddError("Avatar is empty!");
                }

            }
            else
            {
                //check user name
                if (customerRequestDto.UserName.IsNullOrEmpty())
                {
                    AddError("User name cannot be empty!");
                }

                //check name
                if (!customerRequestDto.Name.IsNullOrEmpty())
                {
                    if (Regex.IsMatch(customerRequestDto.UserName, @"[^a-zA-Z0-9]"))
                    {
                        AddError("Name cannot contain special characters!");
                    }
                }

                //check date of birth
                if (customerRequestDto.Dob.HasValue)
                {
                    DateOnly minDateOfBirth = new DateOnly(1900, 1, 1);
                    DateOnly maxDateOfBirth = DateOnly.FromDateTime(DateTime.Now); // Or a specific end date if required

                    // Use DateOnly directly from customerRequestDto.Dob
                    DateOnly dob = customerRequestDto.Dob.Value;

                    if (dob < minDateOfBirth || dob > maxDateOfBirth)
                    {
                        AddError("Date of birth is outside the reasonable range");
                    }
                }



                //check gender
                if (!customerRequestDto.Gender.IsNullOrEmpty())
                {
                    if (!customerRequestDto.Gender.Equals("Nam", StringComparison.OrdinalIgnoreCase) && !customerRequestDto.Gender.Equals("Nữ", StringComparison.OrdinalIgnoreCase)
                    && !customerRequestDto.Gender.Equals("Khác", StringComparison.OrdinalIgnoreCase))
                    {
                        AddError("Invalid gender!");
                    }
                }

                //check phone number
                if (!customerRequestDto.PhoneNumber.IsNullOrEmpty())
                {
                    if (!Regex.IsMatch(customerRequestDto.PhoneNumber, @"^\d{10}$"))
                    {
                        AddError("Phone number must contain exactly 10 digits");

                    }
                }

                //check email
                if (!customerRequestDto.Email.IsNullOrEmpty())
                {
                    Customer customer = await 
                        unitOfWork.customerRepo.GetCustomerByPhoneOrEmailOrUsername(customerRequestDto.Email);
                    if (!customer.Email.Equals(customerRequestDto.Email))
                    {
                        if (unitOfWork.userRepo.checkUniqueEmail(customerRequestDto.Email))
                        {
                            AddError("Email is existed!");
                        }
                    }                   
                }
            }


            //status
            if (customerRequestDto.Status == null)
            {
                AddError("Status is empty!");
            }

            //check clinic
            if (customerRequestDto.ClinicId.IsNullOrEmpty())
            {
                AddError("Please choose a clinic!");
            }
            else
            {
                Clinic? clinic = await unitOfWork.clinicRepo.getClinicById(customerRequestDto.ClinicId);
                if (clinic == null)
                {
                    AddError("Clinic does not exist!");
                }
            }


            return responseDTO;
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
        
        public async Task<ResponseDTO> UploadFile(IFormFile file, Guid customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("Upload Customer File Successfully", 200, true, null);
            try
            {
                var model = await unitOfWork.customerRepo.GetCustomerById(customerId);
                if (model == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Customer not found!";
                    responseDTO.StatusCode = 404;
                    responseDTO.Result = null;
                }

                if (model.Avatar != null)
                {
                    // Delete the image before add new one
                    await firebaseStorageService.DeleteFileAndReference(model.Avatar);
                }

                // Generate a unique file name
                var fileName = $"{model.CustomerId}";
        
                // Upload image to Firebase Storage
                var avatar = await firebaseStorageService.UploadFile(fileName, file, "customer");

                // Update the URL in the medical record model
                model.Avatar = avatar;
                model = await unitOfWork.customerRepo.UpdateCustomer(model);
        
                var viewModel = mapper.Map<CustomerDTO>(model);
                responseDTO.Result = viewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return responseDTO;
        }
    
        public async Task<ResponseDTO> DeleteFileAndReference(Guid customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("Delete Customer File Successfully", 200, true, null);

            try
            {
                var model = await unitOfWork.customerRepo.GetCustomerById(customerId);
                if (model == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Customer not found!";
                    responseDTO.StatusCode = 404;
                    responseDTO.Result = null;
                }

                // Delete image to Firebase Storage
                await firebaseStorageService.DeleteFileAndReference(model.Avatar);

                // Update the URL in the medical record model;
                model.Avatar = null;
                model = await unitOfWork.customerRepo.UpdateCustomer(model);

                var viewModel = mapper.Map<CustomerDTO>(model);
                responseDTO.Result = viewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetCustomers(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField,
            string? sortOrder)
        {
            try
            {
                List<Customer> customers = await unitOfWork.customerRepo.GetAllCustomers(pageNumber, rowsPerPage);
                
                
                customers = FilterCustomer(customers, filterField, filterValue);
                customers = SortCustomer(customers, sortField, sortOrder);

                List<CustomerDTO> viewModels = mapper.Map<List<CustomerDTO>>(customers);
                foreach (var viewModel in viewModels)
                {
                    var clinics = customers
                        .FirstOrDefault(x => x.CustomerId == viewModel.CustomerId)?
                        .CustomerClinics
                        .Where(cu => cu.Status==true) 
                        .Select(cu => cu.Clinic)
                        .ToList();
                    viewModel.Clinics = mapper.Map<List<ClinicDTO>>(clinics);
                }
                return new ResponseDTO("Get customer successfully!", 200, true, viewModels);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }
        private List<Customer> FilterCustomer(List<Customer> customers, string filterField, string filterValue)
        {
            if (string.IsNullOrEmpty(filterField) || string.IsNullOrEmpty(filterValue))
            {
                return customers;
            }
            
            switch (filterField.ToLower())
            {
                case "username":
                    return customers.Where(u => u.UserName.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "dob":
                    if (DateOnly.TryParse(filterValue, out var dob))    
                    {
                        return customers.Where(u => u.Dob == dob).ToList();
                    }
                    break;
                case "name":
                    return customers.Where(u => u.Name != null && u.Name.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "gender":
                    return customers.Where(u => u.Gender != null && u.Gender.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "phonenumber":
                    return customers.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "email":
                    return customers.Where(u => u.Email != null && u.Email.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                case "address":
                    return customers.Where(u => u.Address != null && u.Address.Contains(filterValue, StringComparison.OrdinalIgnoreCase)).ToList();               
                case "status":
                    if (bool.TryParse(filterValue, out var status))
                    {
                        return customers.Where(u => u.Status == status).ToList();
                    }
                    break;               
                case "search":
                    return customers = customers.Where(x =>
                        x.UserName.ToLower().Contains(filterValue) ||
                        x.Name.ToLower().Contains(filterValue) ||
                        (x.PhoneNumber != null && x.PhoneNumber.Contains(filterValue)) ||
                        (x.Email != null && x.Email.Contains(filterValue))
                    ).ToList();
                case "clinic" : 
                    return customers = customers
                        .Where(user => user.CustomerClinics.Any(cu => cu.ClinicId.Equals(Guid.Parse(filterValue)) && cu.Status == true))
                        .ToList();
                default:
                    return customers;
            }
            return customers;
        }

        private List<Customer> SortCustomer(List<Customer> customers, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField) || string.IsNullOrEmpty(sortOrder))
            {
                return customers;
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            switch (sortField.ToLower())
            {
                case "username":
                    return isAscending ? customers.OrderBy(u => u.UserName).ToList() : customers.OrderByDescending(u => u.UserName).ToList();
                case "name":
                    return isAscending ? customers.OrderBy(u => u.Name).ToList() : customers.OrderByDescending(u => u.Name).ToList();
                case "dob":
                    return isAscending ? customers.OrderBy(u => u.Dob).ToList() : customers.OrderByDescending(u => u.Dob).ToList();
                case "gender":
                    return isAscending ? customers.OrderBy(u => u.Gender).ToList() : customers.OrderByDescending(u => u.Gender).ToList();
                case "phonenumber":
                    return isAscending ? customers.OrderBy(u => u.PhoneNumber).ToList() : customers.OrderByDescending(u => u.PhoneNumber).ToList();
                case "email":
                    return isAscending ? customers.OrderBy(u => u.Email).ToList() : customers.OrderByDescending(u => u.Email).ToList();
                case "address":
                    return isAscending ? customers.OrderBy(u => u.Address).ToList() : customers.OrderByDescending(u => u.Address).ToList();
                case "status":
                    return isAscending ? customers.OrderBy(u => u.Status).ToList() : customers.OrderByDescending(u => u.Status).ToList();
            }

            return customers;
        }
    }
}
