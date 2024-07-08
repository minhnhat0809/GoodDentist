﻿using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace GoodDentist.Controllers
{
    [Route("api/examinations")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService examinationService;
        public ExaminationController(IExaminationService examinationService) 
        {
            this.examinationService = examinationService;
        }

        [HttpGet("examination-detail")]
        public async Task<ActionResult<ResponseDTO>> GetExaminationDetail([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.GetExaminationById(examId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("all-examinations-of-clinic")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfClinic([FromQuery] string clinicId, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfClinic(clinicId, pageNumber, 
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/clinic/user")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfUser([FromQuery] string clinicId, [FromQuery] string userId, [FromQuery] string actor , [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfUser(clinicId, userId, actor, pageNumber, 
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpGet("/clinic/profile")]
        public async Task<ActionResult<ResponseDTO>> GetAllExaminationsOfExaminationProfile([FromQuery] int profileId, [FromQuery] string userId, [FromQuery] string actor, [FromQuery] int pageNumber, [FromQuery] int rowsPerPage,
           [FromQuery] string? filterField = null,
           [FromQuery] string? filterValue = null,
           [FromQuery] string? sortField = null,
           [FromQuery] string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = await examinationService.GetAllExaminationOfExaminationProfile(profileId, pageNumber,
                rowsPerPage, filterField, filterValue, sortField, sortOrder);

            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPost("/new-examination")]
        public async Task<ActionResult<ResponseListDTO>> CreateExamination([FromBody] ExaminationRequestDTO examinationDTO)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.CreateExamination(examinationDTO, mod);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpPut("examination")]
        public async Task<ActionResult<ResponseListDTO>> UpdateExamination([FromBody] ExaminationRequestDTO examinationDTO)
        {
            string mod = "c";
            ResponseListDTO responseDTO = await examinationService.UpdateExamination(examinationDTO, mod);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }

        [HttpDelete("examination")]
        public async Task<ActionResult<ResponseDTO>> DeleteExamination([FromQuery] int examId)
        {
            ResponseDTO responseDTO = await examinationService.DeleteExamination(examId);
            return StatusCode(responseDTO.StatusCode, responseDTO);
        }
    }
}
