﻿using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
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

        public async Task<ResponseListDTO> CreateExamination(ExaminationRequestDTO examinationDTO, string mod, string mode, string customerId)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;
            try
            {
                responseListDTO = await ValidateExamination(examinationDTO, mod, mode);
                if (responseListDTO.Message.Count > 0)
                {
                    return responseListDTO;
                }

                if (mode.Equals("new", StringComparison.OrdinalIgnoreCase))
                {
                    if (customerId.IsNullOrEmpty())
                    {
                        responseListDTO.Message.Add("Customer Id is null!");
                        responseListDTO.StatusCode = 400;
                        responseListDTO.IsSuccess = false;
                        return responseListDTO;
                    }
                    else
                    {
                        Customer? customer = await unitOfWork.customerRepo.GetByIdAsync(Guid.Parse(customerId));
                        if (customer == null)
                        {
                            responseListDTO.Message.Add("This customer is not exist!");
                            responseListDTO.StatusCode = 400;
                            responseListDTO.IsSuccess = false;
                            return responseListDTO;
                        }
                    }

                    ExaminationProfile examinationProfile = new ExaminationProfile();
                    examinationProfile.Status = true;
                    examinationProfile.Date = DateOnly.FromDateTime(DateTime.Now);
                    examinationProfile.Diagnosis = "Đang cập nhật";
                    examinationProfile.CustomerId = Guid.Parse(customerId);

                    await unitOfWork.examProfileRepo.CreateAsync(examinationProfile);

                    Examination examinationForNew = mapper.Map<Examination>(examinationDTO);
                    examinationForNew.ExaminationProfileId = examinationProfile.ExaminationProfileId;

                    await unitOfWork.examinationRepo.CreateAsync(examinationForNew);

                    ExaminationDTO ExamDTO = mapper.Map<ExaminationDTO>(examinationForNew);
                    
                    responseListDTO.Result = ExamDTO;
                    return responseListDTO;
                }

                Examination examination = mapper.Map<Examination>(examinationDTO);

                await unitOfWork.examinationRepo.CreateAsync(examination);

                ExaminationDTO examDTO = mapper.Map<ExaminationDTO>(examination);
                
                responseListDTO.Result = examDTO;
                return responseListDTO;
            }catch (Exception ex)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(ex.Message);
                responseListDTO.StatusCode = 500;
                return responseListDTO;
            }
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

        public async Task<ResponseDTO> GetAllExaminationOfClinic(string clinicId, int pageNumber, int rowsPerPage, 
            string? sortField = null, string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);           
            try
            {
                List<Examination> examinations = await unitOfWork.examinationRepo.GetAllExaminationOfClinic(clinicId, pageNumber, rowsPerPage);
                responseDTO.Result = examinations;
                return responseDTO;

            }catch (Exception ex)
            {
                responseDTO.IsSuccess=false;
                responseDTO.Message=ex.Message;
                responseDTO.StatusCode=500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> GetAllExaminationOfExaminationProfile(int examProfileId, int pageNumber, 
            int rowsPerPage, string? sortField = null, string? sortOrder = "asc")
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                if (examProfileId <= 0)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data !";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }
                
                ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById(examProfileId);
                if (examinationProfile == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data !";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                List<Examination> examinations = new List<Examination>();

                examinations = await unitOfWork.examinationRepo.GetExaminationByProfileId(examProfileId, pageNumber, rowsPerPage);

                List<ExaminationDTO> examinationDTOs = mapper.Map<List<ExaminationDTO>>(examinations);
                foreach (var ex in examinationDTOs)
                {
                    string dentistName = unitOfWork.userRepo.getUserName(ex.ExaminationProfile.DentistId.ToString());
                    string customerName = await unitOfWork.customerRepo.GetCustomerName(ex.ExaminationProfile.CustomerId.ToString());
                    ex.DentistName = dentistName;
                    ex.CustomerName = customerName;
                    ex.CustomerId = ex.ExaminationProfile.CustomerId.ToString();


                }
                responseDTO.Result = examinationDTOs;
                responseDTO.Message = "Get successfully!";
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

        public async Task<ResponseDTO> GetAllExaminationOfUser(string clinicId, string userId, string actor, DateOnly selectedDate,
            int pageNumber, int rowsPerPage,
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
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }
                else if (actor.Equals("dentist", StringComparison.OrdinalIgnoreCase))
                {
                    examinations = await unitOfWork.examinationRepo.GetAllExaminationOfDentist(clinicId, userId, selectedDate, pageNumber, rowsPerPage);

                }
                else if (actor.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    examinations = await unitOfWork.examinationRepo.GetAllExaminationOfCustomer(clinicId, userId, pageNumber, rowsPerPage);
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Bad request data!";
                    responseDTO.StatusCode = 400;
                    return responseDTO;
                }

                List<ExaminationDTO> examinationDTOs = new List<ExaminationDTO>();

                foreach (var e in examinations)
                {
                    ExaminationDTO examinationDTO = mapper.Map<ExaminationDTO>(e);
                    examinationDTO.CustomerId = e.ExaminationProfile.CustomerId.ToString();
                    examinationDTO.CustomerName = e.ExaminationProfile.Customer.Name;
                    examinationDTOs.Add(examinationDTO);
                }

                responseDTO.Result = examinationDTOs;
                responseDTO.Message = "Get successfully!";
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.StatusCode = 500;
                
            }
            return responseDTO;           
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
                examinationDTO.Orders = mapper.Map<List<OrderDTO>>(examination.Orders);
                examinationDTO.MedicalRecords = mapper.Map<List<MedicalRecordDTO>>(examination.MedicalRecords);
                examinationDTO.Prescriptions = mapper.Map<List<PrescriptionDTO>>(examination.Prescriptions);

                User? dentist = await unitOfWork.userRepo.GetByIdAsync(examination.DentistId);
                if (dentist != null) examinationDTO.DentistName = dentist.Name;

                examinationDTO.DentistSlot = mapper.Map<DentistslotDTO>(examination.DentistSlot);
                examinationDTO.DentistSlot.Dentist = mapper.Map<UserDTO>(dentist);

                Room? room = await unitOfWork.roomRepo.GetByIdAsync(examination.DentistSlot.RoomId);
                if (room != null) examinationDTO.DentistSlot.Room = mapper.Map<RoomDTO>(room);

                ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById(examinationDTO.ExaminationProfileId.Value);
                if (examinationProfile != null)
                {
                    examinationDTO.ExaminationProfile = mapper.Map<ExaminationProfileDTO>(examinationProfile);

                    Customer? customer = await unitOfWork.customerRepo.GetByIdAsync(examinationProfile.CustomerId);
                    User? dentist1 = await unitOfWork.userRepo.GetByIdAsync(examinationProfile.DentistId);
                    if (customer != null)
                    {
                        examinationDTO.ExaminationProfile.Customer = mapper.Map<UserDTO>(customer);
                        examinationDTO.ExaminationProfile.Dentist = mapper.Map<UserDTO>(dentist1);
                        examinationDTO.CustomerName = customer.Name;
                        examinationDTO.CustomerId = customer.CustomerId.ToString();
                    }
                }

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

        public async Task<ResponseListDTO> UpdateExamination(ExaminationRequestDTO examinationDTO, string mod)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;
            try
            {
                responseListDTO = await ValidateExamination(examinationDTO, mod, "");
                if (responseListDTO.Message.Count > 0)
                {
                    return responseListDTO;
                }

                Examination examination = await unitOfWork.examinationRepo.GetExaminationById(examinationDTO.ExaminationId.Value);
                examination.ExaminationProfileId = examinationDTO.ExaminationProfileId;
                examination.DentistSlotId = examinationDTO.DentistSlotId;
                examination.TimeStart = examinationDTO.TimeStart;
                examination.TimeEnd = examinationDTO.TimeEnd;
                examination.Status = examinationDTO.Status;
                examination.Diagnosis = examinationDTO.Diagnosis;

                await unitOfWork.examinationRepo.UpdateAsync(examination);
                return responseListDTO;
            }
            catch (Exception ex)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(ex.Message);
                responseListDTO.StatusCode = 500;
                return responseListDTO;
            }
        }

        private async Task<ResponseListDTO> ValidateExamination(ExaminationRequestDTO examinationDTO, string mod, string mode)
        {
            ResponseListDTO responseListDTO = new ResponseListDTO();
            responseListDTO.IsSuccess = true;
            responseListDTO.StatusCode = 200;

            void Add(string error)
            {
                responseListDTO.IsSuccess = false;
                responseListDTO.Message.Add(error);
                responseListDTO.StatusCode = 400;
            }

            if (mod.Equals("u"))
            {
                if (examinationDTO.ExaminationId <= 0) Add("Bad request ID data!");
                else
                {
                    Examination? examination = await unitOfWork.examinationRepo.GetExaminationById(examinationDTO.ExaminationId.Value);
                    if (examination == null) Add("Examination is not existed!");
                }

                if (examinationDTO.ExaminationProfileId <= 0) Add("Profile ID is null!");
                else
                {
                    ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById((int)examinationDTO.ExaminationProfileId);
                    if(examinationProfile == null) Add("Profile is not existed!");
                }
            }else if (mod.Equals("c"))
            {
                if (mode.Equals("old", StringComparison.OrdinalIgnoreCase))
                {
                    if (examinationDTO.ExaminationProfileId <= 0) Add("Profile ID is null!");
                    else
                    {
                        ExaminationProfile examinationProfile = await unitOfWork.examProfileRepo.GetExaminationProfileById((int)examinationDTO.ExaminationProfileId);
                        if (examinationProfile == null) Add("Profile is not existed!");
                    }
                }

                if (examinationDTO.Notes.IsNullOrEmpty())
                {
                    Add("Please input notes for dentist!");
                }
            }

            if (examinationDTO.Diagnosis.IsNullOrEmpty()) Add("Diagnose is empty!");


            if (!examinationDTO.Status.HasValue) Add("Status is empty!");

            if (examinationDTO.DentistSlotId <= 0) 
            {
                Add("Dentist slot ID is empty!");
            }
            else
            {
                DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByID(examinationDTO.DentistSlotId.Value);
                if (dentistSlot == null)
                {
                    Add("Dentist slot is not exist!");
                }
                else
                {
                    string check = await CheckAvailableSlot((int)examinationDTO.DentistSlotId, examinationDTO.TimeStart, examinationDTO.TimeEnd);
                    if (!check.IsNullOrEmpty())
                    {
                        Add(check);
                    }
                    else
                    {
                        var examinations = dentistSlot.Examinations.ToList();
                        if (examinations.Count > 0)
                        {
                            foreach(var e in examinations)
                            {
                                if ((examinationDTO.TimeStart >= e.TimeStart && examinationDTO.TimeStart < e.TimeEnd) || 
                                    (examinationDTO.TimeStart < e.TimeStart && examinationDTO.TimeEnd > e.TimeStart))
                                {
                                    Add("There is an appointment at :" + e.TimeStart + "-" + e.TimeEnd);
                                    break;
                                }
                            }
                        }
                    }
                }
                
            }
            return responseListDTO;
        }

        private async Task<string> CheckAvailableSlot(int dentistSlotId, DateTime TimeStart, DateTime TimeEnd)
        {
            string result = "";
            DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetByIdAsync(dentistSlotId);

            if (TimeStart >= TimeEnd)
            {
                return "Time Start must be smaller than Time End!";
            }

            if (dentistSlot == null)
            {
                return "This dentist slot is not existed!";
            }

            if (!(dentistSlot.TimeStart <= TimeStart && dentistSlot.TimeEnd >= TimeEnd))
            {
                return "Time must be in range [" + dentistSlot.TimeStart +","+ dentistSlot.TimeEnd +"]";
            }
            return result;
        }
        
        public async Task<ResponseDTO> GetAllExaminationTest(int pageNumber, int rowsPerPage, string? filterField, string? filterValue, string? sortField,
            string? sortOrder)
        {
            try
            {
                List<Examination> models = await unitOfWork.examinationRepo.GetAllExaminationsTest(pageNumber, rowsPerPage);

                
                
                List<ExaminationDTO> viewModels = mapper.Map<List<ExaminationDTO>>(models);
                foreach (var viewModel in viewModels)
                {
                    var customer = models.FirstOrDefault(x => x.ExaminationId == viewModel.ExaminationId).ExaminationProfile.Customer;
                    viewModel.CustomerId = customer.CustomerId.ToString();
                    viewModel.CustomerName = customer.Name;
                }
                return new ResponseDTO("Get Examinations successfully!", 200, true, viewModels);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(ex.Message, 500, false, null);
            }
        }
    }
}
