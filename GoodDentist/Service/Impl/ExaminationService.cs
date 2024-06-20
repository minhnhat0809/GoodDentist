using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationRepo examinationRepo;
        private readonly IMapper mapper;
        public ExaminationService(IExaminationRepo examinationRepo, IMapper mapper) 
        { 
            this.examinationRepo = examinationRepo;
            this.mapper = mapper;
        }

        public Task<ResponseListDTO> CreateExamination(ExaminationDTO examinationDTO)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();

            void Add(string error)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(error);
            }



            throw new NotImplementedException();
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

                Examination? examination = await examinationRepo.GetExaminationById(examId);

                if (examination == null)
                {
                    responseDTO.Message = "Exam is not found.";
                    responseDTO.IsSuccess = true;
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
    }
}
