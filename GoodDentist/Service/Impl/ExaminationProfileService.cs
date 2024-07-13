using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObject.DTO.ExaminationDTOs.View;
using BusinessObject.DTO.ExaminationProfileDTOs.View;

namespace Services.Impl
{
    public class ExaminationProfileService : IExaminationProfileService
    {
        private readonly IExamProfileRepo examProfileRepo;
        private readonly IMapper mapper;

        public ExaminationProfileService(IExamProfileRepo examProfileRepo, IMapper mapper)
        {
            this.examProfileRepo = examProfileRepo;
            this.mapper = mapper;
        }

        public async Task<ResponseDTO> GetExaminationProfilesByCustomerId(string customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("Get Examination Successfully", 200, true, null);
            try
            {
                List<ExaminationProfile> examinationProfiles = await examProfileRepo.GetProfilesByCustomerId(customerId);
                responseDTO.Result = mapper.Map<List<ExaminationProfileDTO>>(examinationProfiles);
            }
            catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetAllExaminationProfiles()
        {
            ResponseDTO responseDTO = new ResponseDTO("Get All Examinations Successfully", 200, true, null);
            try
            {
                List<ExaminationProfile> examinationProfiles = await examProfileRepo.GetAllExaminationProfiles();
                responseDTO.Result = examinationProfiles;
            }
            catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> CreateExaminationProfile(ExaminationProfileForExamDTO examinationProfileDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO("Create Examination Successfully", 201, true, null);
            try
            {
                ExaminationProfile examinationProfile = mapper.Map<ExaminationProfile>(examinationProfileDTO);
                await examProfileRepo.CreateExaminationProfile(examinationProfile);
                responseDTO.Result = examinationProfile;
            }
            catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> UpdateExaminationProfile(ExaminationProfileForExamDTO examinationProfileDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO("Update Examination Successfully", 200, true, null);
            try
            {
                ExaminationProfile examinationProfile = mapper.Map<ExaminationProfile>(examinationProfileDTO);
                await examProfileRepo.UpdateExaminationProfile(examinationProfile);
                responseDTO.Result = examinationProfile;
            }
            catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> DeleteExaminationProfile(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO("Delete Examination Successfully", 200, true, null);
            try
            {
                await examProfileRepo.DeleteExaminationProfile(id);
            }
            catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
    }
}
