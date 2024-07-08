using BusinessObject.DTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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

                User dentist = await unitOfWork.userRepo.GetByIdAsync(dentistId);
                if (dentist == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "Dentist is not exist!";
                    return responseDTO;
                }

                //List<MedicalRecord> medicalRecords = unitOfWork.examinationRepo.GetAllExaminationOfDentist(dentistId);



                responseDTO.Result = null;
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
    }
}
