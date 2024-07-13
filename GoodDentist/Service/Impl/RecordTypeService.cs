using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.RecordTypeDTOs;
using BusinessObject.DTO.RecordTypeDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;

namespace Services.Impl
{
    public class RecordTypeService : IRecordTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RecordTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ResponseDTO> AddRecordType(RecordTypeCreateDTO recordTypeDTO)
        {
            try
            {
                var check = await CheckValidationAddRecordType(recordTypeDTO);
                if (check.IsSuccess == false)
                {
                    return check;
                }

                RecordType recordType = mapper.Map<RecordType>(recordTypeDTO);
                await unitOfWork.recordTypeRepo.CreateAsync(recordType);
                return new ResponseDTO("Creat succesfully", 200, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        public async Task<ResponseDTO> DeleteRecordType(int recordTypeId)
        {
            try
            {
                var recordType = await unitOfWork.recordTypeRepo.GetByIdAsync(recordTypeId);
                if (recordType == null)
                {
                    return new ResponseDTO("This Record Type is not exist!", 400, false, null);
                }
                recordType.Status = false;
                var result = await unitOfWork.recordTypeRepo.DeleteAsync(recordType);
                if (result)
                {
                    return new ResponseDTO("Record Type Delete succesfully!", 201, true, null);
                }
                return new ResponseDTO("Record Type Delete unsucessfully!", 400, false, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        public async Task<ResponseDTO> GetAllRecordTyoe(int pageNumber, int pageSize)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 300, true, null);
            try
            {
                List<RecordType>? recordTypeList = await unitOfWork.recordTypeRepo.GetAllRecordType(pageNumber, pageSize);

                var all = recordTypeList.Where(c => c.Status == true || c.Status == null);
                List<RecordTypeDTO> recordTypeDTOList = mapper.Map<List<RecordTypeDTO>>(all);

                responseDTO.Message = "Get all Medicine successfully!";
                responseDTO.Result = recordTypeDTOList;
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

        public async Task<ResponseDTO> SearchRecordType(string searchValue)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 300, true, null);
            try
            {
                List<RecordType>? recordTypeList = await unitOfWork.recordTypeRepo.FindByConditionAsync(c => c.RecordName == searchValue);

                var all = recordTypeList.Where(c => c.Status == true || c.Status == null);
                List<RecordTypeDTO> recordTypeDTOList = mapper.Map<List<RecordTypeDTO>>(all);

                responseDTO.Message = "Get all Medicine successfully!";
                responseDTO.Result = recordTypeDTOList;
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

        public async Task<ResponseDTO> UpdateRecordType(RecordTypeDTO recordTypeDTO)
        {
            try
            {
                var recordType = await unitOfWork.recordTypeRepo.GetByIdAsync(recordTypeDTO.RecordTypeId);
                if (recordType == null)
                {
                    return new ResponseDTO("This Record Type is not exist!", 400, false, null);
                }
                var check = await CheckValidationUpdateRecordType(recordTypeDTO);
                if (check.IsSuccess == false)
                {
                    return check;
                }
                int recordTypeId = recordType.RecordTypeId;

                unitOfWork.recordTypeRepo.Detach(recordType);

                var updateRecordType = mapper.Map<RecordType>(recordTypeDTO);

                updateRecordType.RecordTypeId = recordTypeId;

                unitOfWork.recordTypeRepo.Attach(updateRecordType);

                await unitOfWork.recordTypeRepo.UpdateAsync(updateRecordType);
                return new ResponseDTO("Update Sucessfully!", 201, true, null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }

        public async Task<ResponseDTO> CheckValidationAddRecordType(RecordTypeCreateDTO recordTypeDTO)
        {

            if (recordTypeDTO.RecordName.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input Record Type name", 400, false, null);
            }

            List<RecordType> recordTypes = await unitOfWork.recordTypeRepo.FindByConditionAsync(c => c.Status == true);
            if (recordTypes.Any(c => c.RecordName == recordTypeDTO.RecordName))
            {
                return new ResponseDTO("Medicine name is already existed!", 400, false, null);
            }
            return new ResponseDTO("Check validation successfully", 200, true, null);
        }

        public async Task<ResponseDTO> CheckValidationUpdateRecordType(RecordTypeDTO recordTypeDTO)
        {
            if (recordTypeDTO.RecordTypeId.ToString().IsNullOrEmpty())
            {
                return new ResponseDTO("Please input Record Type id", 400, false, null);
            }
            if (recordTypeDTO.RecordName.IsNullOrEmpty())
            {
                return new ResponseDTO("Please input Record Type name", 400, false, null);
            }

            List<RecordType> recordTypesList = await unitOfWork.recordTypeRepo.FindByConditionAsync(c => c.Status == true);
            if (recordTypesList.Any(c => c.RecordName == recordTypeDTO.RecordName && c.RecordTypeId != recordTypeDTO.RecordTypeId))
            {
                return new ResponseDTO("Record Type name is already existed!", 400, false, null);
            }
            return new ResponseDTO("Check validation successfully", 200, true, null);
        }
    }
}
