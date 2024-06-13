using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Repositories;

namespace Services.Impl
{
    public class ExaminationService : IExaminationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ResponseDTO _Response;
        public ResponseListDTO _ResponseList;
        private List<String> stringList = null;
        public ExaminationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _Response = new ResponseDTO("", 200, true, null);
            //_ResponseList = new ResponseListDTO( List<String> stringList , 200, true, null);

        }
        // khong biet xai ResponseList

        public async Task<ResponseDTO> GetExamination(int id)
        {
            try
            {
                var model = await _unitOfWork.examinationRepo.GetByIdAsync(id);
                _Response.Result = _mapper.Map<ExaminationDTO>(model);
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _Response;
        }

        public async Task<ResponseDTO> GetExaminations(
            )
        {
            try
            {
                var models = await _unitOfWork.examinationRepo.FindAllAsync();
                _Response.Result = _mapper.Map<List<ExaminationDTO>>(models);
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _Response;
        }

        public async Task<ResponseListDTO> CreateExamination(ExaminationRequestDTO requestDto)
        {
            try
            {
                var model = await _unitOfWork.examinationRepo.GetByIdAsync(requestDto.ExaminationId);
                if (model == null)
                {
                    model = _mapper.Map<Examination>(requestDto);
                    _unitOfWork.examinationRepo.CreateAsync(model);
                    _Response.Result = _mapper.Map<ExaminationDTO>(model);
                }
                else
                {
                    _Response.IsSuccess = false;
                    _Response.Message = "There is existed Examination!";
                    _Response.StatusCode = 404;
                }
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _ResponseList;
        }

        public async Task<ResponseDTO> DeleteExamination(int id)
        {
            try
            {
                var model = await _unitOfWork.examinationRepo.GetByIdAsync(id);
                if(model != null)
                {
                    _unitOfWork.examinationRepo.DeleteAsync(model);
                    _Response.Result = _mapper.Map<ExaminationDTO>(model);
                }
                else
                {
                    _Response.IsSuccess = false;
                    _Response.Message = "Not Found Examination!";
                    _Response.StatusCode = 500;   
                }
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _Response;
        }

        public async Task<ResponseListDTO> UpdateExamination(ExaminationRequestDTO requestDto)
        {
            try
            {
                var model = await _unitOfWork.examinationRepo.GetByIdAsync(requestDto.ExaminationId);
                if(model != null)
                {
                    model = _mapper.Map<Examination>(requestDto);
                    _unitOfWork.examinationRepo.UpdateAsync(model);
                    _Response.Result = _mapper.Map<ExaminationDTO>(model);
                }
                else
                {
                    _Response.IsSuccess = false;
                    _Response.Message = "Not Found Examination!";
                    _Response.StatusCode = 500;   
                }
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _ResponseList;
        }
        // khong biet
        public async Task<ResponseDTO> GetAllExaminationsOfClinic(Guid clinicId)
        {
            try
            {
                var models = await _unitOfWork.examinationRepo.FindByConditionAsync(x=>x.DentistId==clinicId);
                _Response.Result = _mapper.Map<List<ExaminationDTO>>(models);
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _Response;
        }
        // khong biet
        public async Task<ResponseDTO> GetAllExaminationsOfUser(Guid userId)
        {
            try
            {
                var models = await _unitOfWork.examinationRepo.FindByConditionAsync(x=>x.DentistId==userId);
                _Response.Result = _mapper.Map<List<ExaminationDTO>>(models);
            }
            catch (Exception e)
            {
                _Response.IsSuccess = false;
                _Response.Message = e.Message;
                _Response.StatusCode = 500;
            }

            return _Response;
        }
    }
}
