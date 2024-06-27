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

        public async Task<ResponseListDTO> CreateExamination(ExaminationDTO examinationDTO)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();

            responseListDTO = await ValidateExamination(examinationDTO);
            if (responseListDTO.Message.Count > 0)
            {
                return responseListDTO;
            }
            
            throw new NotImplementedException();
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
                    responseDTO.Message = "Internal error at request data !";
                    return responseDTO;
                }
                
                ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById(examProfileId);
                if (examinationProfile == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Internal error at request data !";
                    return responseDTO;
                }

                List<Examination> examinations = new List<Examination>();

                examinations = await unitOfWork.examinationRepo.GetExaminationByProfileId(examProfileId, pageNumber, rowsPerPage);

                List<ExaminationDTO> examinationDTOs = mapper.Map<List<ExaminationDTO>>(examinations);
                responseDTO.Result = examinationDTOs;
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
                    responseDTO.Message = "Error at request data!";
                    responseDTO.StatusCode = 500;
                    return responseDTO;
                }

                List<ExaminationDTO> examinationDTOs = mapper.Map<List<ExaminationDTO>>(examinations);
                responseDTO.Result = examinationDTOs;
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

        private async Task<ResponseListDTO> ValidateExamination(ExaminationDTO examinationDTO)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            
            void Add(string error)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(error);
            }

            

            return responseListDTO;
        }
    }
}
