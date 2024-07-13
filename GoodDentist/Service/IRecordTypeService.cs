using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.RecordTypeDTOs;
using BusinessObject.DTO.RecordTypeDTOs.View;

namespace Services
{
    public interface IRecordTypeService
    {
        Task<ResponseDTO> GetAllRecordTyoe(int pageNumber, int pageSize);

        Task<ResponseDTO> SearchRecordType(string searchValue);

        Task<ResponseDTO> AddRecordType(RecordTypeCreateDTO recordTypeDTO);

        Task<ResponseDTO> UpdateRecordType(RecordTypeDTO recordTypeDTO);

        Task<ResponseDTO> DeleteRecordType(int recordTypeId);
    }
}
