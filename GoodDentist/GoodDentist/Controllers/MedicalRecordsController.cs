using System;
using Microsoft.AspNetCore.Mvc;
using BusinessObject.DTO;
using Services;
using BusinessObject.DTO.MedicalRecordDTOs;

namespace GoodDentist.Controllers
{
    [Route("api/medical-records")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public ResponseDTO ResponseDto;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
            
        }

        // GET: api/MedicalRecords
        [HttpGet]
        public async Task<ResponseDTO> GetMedicalRecords()
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dtoList = await _medicalRecordService.GetRecords();
                ResponseDto.Result = dtoList;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
            }

            return ResponseDto;
        }

        // GET: api/MedicalRecords/5
        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetMedicalRecord(int id)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dto = await _medicalRecordService.GetRecord(id);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
            }

            return ResponseDto;
        }

        // PUT: api/MedicalRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ResponseDTO> PutMedicalRecord([FromBody]MedicalRecordRequestDTO medicalRecord)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dto = await _medicalRecordService.UpdateRecord(medicalRecord);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
            }

            return ResponseDto;
        }

        // POST: api/MedicalRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ResponseDTO> PostMedicalRecord(MedicalRecordRequestDTO medicalRecord)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dto = await _medicalRecordService.CreateRecord(medicalRecord);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
            }

            return ResponseDto;
            
        }

        // DELETE: api/MedicalRecords/5
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteMedicalRecord(int id)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dto = await _medicalRecordService.DeleteRecord(id);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
            }

            return ResponseDto;
        }
        
        // POST: api/MedicalRecords/delete
        [HttpPost("delete-file/{id:int}")]
        public async Task<ResponseDTO> DeleteFile(int id)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            try
            {
                var dto = await _medicalRecordService.DeleteFileAndReference(id);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
                ResponseDto.StatusCode = 500;
            }
            return ResponseDto;
        }
        [HttpPost("upload-file/{id:int}")]
        public async Task<ResponseDTO> UploadFile(IFormFile file, int id)
        {
            ResponseDto = new ResponseDTO("", 200, true,null);
            if (file == null || file.Length == 0)
            {
                ResponseDto.Message = "File is empty!";
                ResponseDto.IsSuccess = false;
                ResponseDto.StatusCode = 500;
            }

            try
            {
                var dto = await _medicalRecordService.UploadFile(file, id);
                ResponseDto.Result = dto;
            }
            catch (Exception e)
            {
                ResponseDto.Message = e.Message;
                ResponseDto.IsSuccess = false;
                ResponseDto.StatusCode = 500;
            }

            return ResponseDto;
        }
        
    }
}
