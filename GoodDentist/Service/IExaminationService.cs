using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public interface IExaminationService
    {
        Task<ResponseDTO> GetExaminationById(int examId);

        Task<ResponseListDTO> CreateExamination(ExaminationDTO examinationDTO);

        Task<ResponseDTO> DeleteExamination(int examId);

        Task<ResponseDTO> GetAllExaminationOfUser(string clinicId, string userId, string actor, int pageNumber, int rowsPerPage,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc");

        Task<ResponseDTO> GetAllExaminationOfClinic(string clinicId, int pageNumber, int rowsPerPage,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc");

        Task<ResponseDTO> GetAllExaminationOfExaminationProfile(int examProfileId, int pageNumber, int rowsPerPage,
           string? filterField = null,
           string? filterValue = null,
           string? sortField = null,
           string? sortOrder = "asc");
    }
}
