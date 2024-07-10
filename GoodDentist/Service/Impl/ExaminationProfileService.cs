using BusinessObject.DTO;
using BusinessObject.Entity;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class ExaminationProfileService : IExaminationProfileService
    {
        private readonly IExamProfileRepo examProfileRepo;

        public ExaminationProfileService(IExamProfileRepo examProfileRepo)
        {
            this.examProfileRepo = examProfileRepo;
        }

        public async Task<ResponseDTO> GetExaminationProfilesByCustomerId(string customerId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<ExaminationProfile> examinationProfiles = await examProfileRepo.GetProfilesByCustomerId(customerId);
                responseDTO.Result = examinationProfiles;
            }catch (Exception ex)
            {
                responseDTO.StatusCode = 500;
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;

            }
            return responseDTO;
        }
    }
}
