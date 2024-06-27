using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class ExaminationService : IExaminationService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public ExaminationService( IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResponseListDTO> CreateExamination(ExaminationDTO examinationDTO, string mod)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;
            try
            {
                responseListDTO = await ValidateExamination(examinationDTO, mod);
                if (responseListDTO.Message.Count > 0)
                {
                    return responseListDTO;
                }


                return responseListDTO;
            }catch (Exception ex)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(ex.Message);
                responseListDTO.StatusCode = 500;
                return responseListDTO;
            }
        }

        public async Task<ResponseDTO> DeleteExamination(int examId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            if (examId <= 0)
            {
                responseDTO.Message = "Exam ID is null.";
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }


            Examination? examination = await unitOfWork.examinationRepo.GetExaminationById(examId);
            if (examination == null)
            {
                responseDTO.Message = "Examination is not found!";
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 404;
                return responseDTO;
            }

            await unitOfWork.examinationRepo.DeleteAsync(examination);
            responseDTO.Message = "Delete successfully!";
            return responseDTO;

        }

        public async Task<ResponseDTO> GetAllExaminationOfClinic(string clinicId, int pageNumber, int rowsPerPage, string? filterField = null, string? filterValue = null, string? sortField = null, string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);           
            try
            {
                List<Examination> examinations = await unitOfWork.examinationRepo.GetAllExaminationOfClinic(clinicId, pageNumber, rowsPerPage);

            }catch (Exception ex)
            {
                responseDTO.IsSuccess=false;
                responseDTO.Message=ex.Message;
                responseDTO.StatusCode=500;
                return responseDTO;
            }
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> GetAllExaminationOfExaminationProfile(int examProfileId, int pageNumber, 
            int rowsPerPage, string? filterField = null, string? filterValue = null, string? sortField = null, string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                if (examProfileId <= 0)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data !";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }
                
                ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById(examProfileId);
                if (examinationProfile == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data !";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                List<Examination> examinations = new List<Examination>();

                examinations = await unitOfWork.examinationRepo.GetExaminationByProfileId(examProfileId, pageNumber, rowsPerPage);

                List<ExaminationDTO> examinationDTOs = mapper.Map<List<ExaminationDTO>>(examinations);
                responseDTO.Result = examinationDTOs;
                responseDTO.Message = "Get successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> GetAllExaminationOfUser(string clinicId, string userId, string actor, int pageNumber, int rowsPerPage,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<Examination> examinations = new List<Examination>();

                if (actor.IsNullOrEmpty())
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Empty actor!";
                    responseDTO.StatusCode = 500;
                    return responseDTO;
                }
                else if (actor.Equals("dentist", StringComparison.OrdinalIgnoreCase))
                {
                    examinations = await unitOfWork.examinationRepo.GetAllExaminationOfDentist(clinicId, userId, pageNumber, rowsPerPage);
                }
                else if (actor.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    examinations = await unitOfWork.examinationRepo.GetAllExaminationOfCustomer(clinicId, userId, pageNumber, rowsPerPage);
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data!";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                List<ExaminationDTO> examinationDTOs = mapper.Map<List<ExaminationDTO>>(examinations);
                responseDTO.Result = examinationDTOs;
                responseDTO.Message = "Get successfully!";
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }           
        }

        public async Task<ResponseDTO> GetExaminationById(int examId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);

            try
            {
                if (examId <= 0)
                {
                    responseDTO.Message = "Exam ID is null.";
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 500;
                    return responseDTO;
                }


                Examination? examination = await unitOfWork.examinationRepo.GetExaminationById(examId);
                if (examination == null)
                {
                    responseDTO.Message = "Examination is not found!";
                    responseDTO.IsSuccess = false;
                    responseDTO.StatusCode = 404;
                    return responseDTO;
                }

                ExaminationDTO examinationDTO = mapper.Map<ExaminationDTO>(examination);
                responseDTO.Result = examinationDTO;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message = ex.Message;
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseListDTO> UpdateExamination(ExaminationDTO examinationDTO, string mod)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;
            try
            {
                responseListDTO = await ValidateExamination(examinationDTO, mod);
                if (responseListDTO.Message.Count > 0)
                {
                    return responseListDTO;
                }


                return responseListDTO;
            }
            catch (Exception ex)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(ex.Message);
                responseListDTO.StatusCode = 500;
                return responseListDTO;
            }
        }

        private async Task<ResponseListDTO> ValidateExamination(ExaminationDTO examinationDTO, string mod)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;

            void Add(string error)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(error);
                responseListDTO.StatusCode = 400;
            }

            if (mod.Equals("u"))
            {
                if (examinationDTO.ExaminationId <= 0) Add("Bad request ID data!");
                else
                {
                    Examination? examination = await unitOfWork.examinationRepo.GetExaminationById(examinationDTO.ExaminationId);
                    if (examination == null) Add("Examination is not existed!");
                }

                if (examinationDTO.ExaminationProfileId <= 0) Add("Profile ID is null!");
                else
                {
                    ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById((int)examinationDTO.ExaminationProfileId);
                    if(examinationProfile == null) Add("Profile is not existed!");
                }
            }else if (mod.Equals("c"))
            {
                if (examinationDTO.Notes.IsNullOrEmpty())
                {
                    Add("Please input notes for dentist!");
                }
            }

            if (examinationDTO.DentistId.IsNullOrEmpty()) Add("Dentist ID is null!");
            else
            {
                User? dentist = await unitOfWork.userRepo.GetByIdAsync(Guid.Parse(examinationDTO.DentistId));
                if (dentist == null)
                {
                    Add("Dentist is not existed!");
                }
            }

            if (examinationDTO.Diagnosis.IsNullOrEmpty()) Add("Diagnose is empty!");


            if (!examinationDTO.Status.HasValue) Add("Status is empty!");

            if (examinationDTO.DentistSlotId <= 0) Add("Dentist slot ID is empty!");

            string check = await CheckAvailableSlot((int)examinationDTO.DentistSlotId, examinationDTO.TimeStart, examinationDTO.TimeEnd);
            if (check.IsNullOrEmpty())
            {
                Add(check);
            }

            return responseListDTO;
        }

        private async Task<string> CheckAvailableSlot(int dentistSlotId, DateTime TimeStart, DateTime TimeEnd)
        {
            string result = "";
            DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetByIdAsync(dentistSlotId);

            if (TimeStart >= TimeEnd)
            {
                return "Time Start must be smaller than Time End!";
            }

            if (dentistSlot == null)
            {
                return "This dentist slot is not existed!";
            }

            if (!(dentistSlot.TimeStart <= TimeStart && dentistSlot.TimeEnd >= TimeEnd))
            {
                return "Time must be in range [" + dentistSlot.TimeStart +","+ dentistSlot.TimeEnd +"]";
            }
            else
            {
                List<Examination> examinations = await unitOfWork.examinationRepo.GetAllExaminationOfDentistSlot(dentistSlotId);
            }


            return result;
        }
    }
}
