using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
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

                List<UserDTO> customerDTO = mapper.Map<List<UserDTO>>(customers);


                responseDTO.Result = customerDTO;
                responseDTO.Message = "Get successfully!";
                return responseDTO;
            }catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                responseDTO.Message = ex.Message;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> GetAllCustomers(string search)
        {
            ResponseDTO responseDTO = new ResponseDTO("",200,true,null);
            try
            {
                string pattern = $@"\b{Regex.Escape(search)}\b";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                List<Customer> customers = await unitOfWork.customerRepo.GetAllCustomers();
                if (!search.IsNullOrEmpty())
                {
                    customers = customers.Where(c => regex.IsMatch(c.Name) || c.PhoneNumber.Contains(search)).ToList();
                }
                

                List<UserDTO> userDTOs = new List<UserDTO>();
                foreach(var u in customers)
                {
                    List<Clinic> clinics = u.CustomerClinics.Select(cc => cc.Clinic).ToList();
                    UserDTO userDTO = mapper.Map<UserDTO>(u);
                    userDTO.Clinics = mapper.Map<List<ClinicDTO>>(clinics);
                    userDTOs.Add(userDTO);
                }

                
                responseDTO.Result = userDTOs;
            }catch (Exception e)
            {
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode =500;
                responseDTO.Message= e.Message;
         
            }
            return responseDTO;
        }

    }
}
