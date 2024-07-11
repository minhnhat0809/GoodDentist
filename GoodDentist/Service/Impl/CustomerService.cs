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

                User dentist = await unitOfWork.userRepo.GetByIdAsync(Guid.Parse(dentistId));
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
    }
}
