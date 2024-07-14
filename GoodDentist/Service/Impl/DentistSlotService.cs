using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.DentistSlotDTOs;
using BusinessObject.DTO.DentistSlotDTOs.View;
using BusinessObject.DTO.UserDTOs.View;
using BusinessObject.Entity;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class DentistSlotService : IDentistSlotService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DentistSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ResponseListDTO> createDentistSlot(List<CreateDentistSlotDTO> dentistSlotDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.IsSuccess = true;
            try
            {
                List<DentistSlot> dentistSlots = new List<DentistSlot>();
                foreach (var dl in dentistSlotDTO)
                {
                    responseDTO = await validateDentistSlot(mapper.Map<UpdateDentistSlotDTO>(dl));
                    if (responseDTO.IsSuccess == true)
                    {
                        DentistSlot dentistSlot = mapper.Map<DentistSlot>(dl);
                        dentistSlots.Add(dentistSlot);
                        await unitOfWork.dentistSlotRepo.CreateAsync(dentistSlot); 
                    }
                }

                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }
                
                responseDTO.Message.Add("Create sucessfully");
                responseDTO.IsSuccess = true;
                responseDTO.Result = mapper.Map<List<DentistSlotDTO>>(dentistSlots);
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message.Add(ex.Message);
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> deleteDentistSlot(int slotId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                if (slotId == 0)
                {
                    responseDTO.Message = "Slot ID is empty!";
                    responseDTO.StatusCode = 400;
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }

                DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByID(slotId);
                if (dentistSlot == null)
                {
                    responseDTO.Message = "This slot is not exist!";
                    responseDTO.StatusCode = 400;
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }

                await unitOfWork.dentistSlotRepo.DeleteAsync(dentistSlot);
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

        public async Task<ResponseDTO> getAllDentistSlots(int pageNumber, int rowsPerPage,
         string sortField, string sortOrder)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo.GetAllDentistSlots(pageNumber, rowsPerPage);

                List<DentistSlotDTO> dentistSlotDTOList = mapper.Map<List<DentistSlotDTO>>(dentistSlotList);
                dentistSlotDTOList = SortDentistSlots(dentistSlotDTOList, sortField, sortOrder);

                responseDTO.Message = "Get all dentistSlot slots successfully!";
                responseDTO.Result = dentistSlotDTOList;
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

        public async Task<ResponseDTO> getAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage,
         string sortField, string sortOrder)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo.GetAllSlotsOfDentist(dentistId, pageNumber, rowsPerPage);
                List<DentistSlotDTO> dentistSlotDTOList = mapper.Map<List<DentistSlotDTO>>(dentistSlotList);
                dentistSlotDTOList = SortDentistSlots(dentistSlotDTOList, sortField, sortOrder);

                responseDTO.Message = "Get all slots of dentistSlot successfully!";
                responseDTO.Result = dentistSlotDTOList;
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

        public async Task<ResponseDTO> getDentistSlotDetail(int slotId)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByID(slotId);
                if (dentistSlot == null)
                {
                    responseDTO.StatusCode = 400;
                    responseDTO.Message = "There are no dentistSlot slot with the ID";
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }

                DentistSlotDTO dentistSlotDTO = mapper.Map<DentistSlotDTO>(dentistSlot);

                responseDTO.Message = "Get dentistSlot slot detail successfully!";
                responseDTO.Result = dentistSlotDTO;
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

        public async Task<ResponseListDTO> updateDentistSlot(UpdateDentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            try
            {
                responseDTO = await validateDentistSlot(dentistSlotDTO);
                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }
                DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByID(dentistSlotDTO.DentistSlotId);
                if (dentistSlot == null)
                {
                    responseDTO.Message.Add("There are no dentist slots with this ID");
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }
                int dentistSlotId = dentistSlot.DentistSlotId;

                unitOfWork.dentistSlotRepo.Detach(dentistSlot);


                DentistSlot updateDentistSlot = mapper.Map<DentistSlot>(dentistSlotDTO);
                updateDentistSlot.DentistSlotId = dentistSlotId;
                unitOfWork.dentistSlotRepo.Attach(updateDentistSlot);

                await unitOfWork.dentistSlotRepo.UpdateAsync(updateDentistSlot);

                responseDTO.Message.Add("Update sucessfully");
                responseDTO.IsSuccess = true;
                responseDTO.Result = mapper.Map<DentistSlotDTO>(dentistSlot);
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.Message.Add(ex.Message);
                responseDTO.IsSuccess = false;
                //responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        private async Task<ResponseListDTO> validateDentistSlot(UpdateDentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.IsSuccess = true;
            bool checkTime = true;
            void AddError(string message)
            {
                responseDTO.Message.Add(message);
                responseDTO.IsSuccess = false;
            }

            if (dentistSlotDTO.ClinicId.IsNullOrEmpty())
            {
                AddError("Internal errror at empty clinic data!");
            }

            if (!dentistSlotDTO.DentistId.HasValue)
            {
                AddError("Please choose a dentist!");
            }
            else
            {
                User? dentist = await unitOfWork.userRepo.GetByIdAsync(dentistSlotDTO.DentistId);
                if (dentist == null)
                {
                    AddError("This dentist does not exist!");
                }
            }

            if (!dentistSlotDTO.TimeStart.HasValue || !dentistSlotDTO.TimeEnd.HasValue)
            {
                AddError("Time start or time end is null!");
                checkTime = false;
            }
            else
            {
                TimeSpan timeStart = dentistSlotDTO.TimeStart.Value.TimeOfDay;
                TimeSpan timeEnd = dentistSlotDTO.TimeEnd.Value.TimeOfDay;
                string error = validateTime(timeStart, timeEnd);
                if (!error.IsNullOrEmpty())
                {
                    AddError(error);
                }
            }

            if (!dentistSlotDTO.Status.HasValue)
            {
                AddError("Status is null!");
            }

            if (!dentistSlotDTO.RoomId.HasValue)
            {
                AddError("Please choose a room!");
            }
            else if (checkTime)
            {
                List<string> errors = await checkRoomAvailable((int)dentistSlotDTO.RoomId, dentistSlotDTO.ClinicId, (DateTime)dentistSlotDTO.TimeStart, dentistSlotDTO.DentistId.ToString());
                if (!errors.IsNullOrEmpty())
                {
                    responseDTO.Message.AddRange(errors);
                    responseDTO.IsSuccess = false;
                }
            }
            return responseDTO;
        }

        private string validateTime(TimeSpan timeStart, TimeSpan timeEnd)
        {
            string errors = "";
            var validSlots = new List<(TimeSpan start, TimeSpan end)>
            {
            (new TimeSpan(8, 0, 0), new TimeSpan(12, 0, 0)),
            (new TimeSpan(13, 0, 0), new TimeSpan(17, 0, 0)),
            (new TimeSpan(17, 0, 0), new TimeSpan(19, 30, 0))
            };

            bool isValid = validSlots.Any(slot => timeStart == slot.start && timeEnd == slot.end);

            if (!isValid)
            {
                errors = "Slot must be in range [8:00 - 12:00], [13:00 - 17:00], [17:00 - 19:30]";
            }
            return errors;
        }

        private async Task<List<string>> checkRoomAvailable(int roomId, string clinicId, DateTime timeStart, string dentistId)
        {
            List<string> errors = new List<string>();
            Room? room = await unitOfWork.roomRepo.GetByIdAsync(roomId);
            if (room == null)
            {
                errors.Add("Room is not existed !!!");
            }else if (!room.ClinicId.Equals(Guid.Parse(clinicId)))
            {
                errors.Add("This clinic does not have this room !!!");
            }

            DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByDentistAndTimeStart(dentistId, timeStart);
            if (dentistSlot != null)
            {
                errors.Add("This dentist already has this slot ["+dentistSlot.TimeStart.Value.TimeOfDay +"-"+dentistSlot.TimeEnd.Value.TimeOfDay+"]");
            }
            else
            {
                DentistSlot? dentistSlotExisted = await unitOfWork.dentistSlotRepo.GetDentistSlotsByRoomAndTimeStart(roomId, timeStart);

                if (dentistSlotExisted != null)
                {
                    string userName = unitOfWork.userRepo.getUserName(dentistSlotExisted.DentistId.ToString());
                    errors.Add("Dentist " + userName + " uses this room in this range time !!!");
                }
            }
            return errors;
        }

        private List<DentistSlotDTO> SortDentistSlots(List<DentistSlotDTO> slots, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField) || string.IsNullOrEmpty(sortOrder))
            {
                return slots;
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            switch (sortField.ToLower())
            {
                case "dentistname":
                    return isAscending ? slots.OrderBy(slot => slot.Dentist.Name).ToList() : slots.OrderByDescending(slot => slot.Dentist.Name).ToList();
                case "timestart":
                    return isAscending ? slots.OrderBy(slot => slot.TimeStart).ToList() : slots.OrderByDescending(slot => slot.TimeStart).ToList();
                case "timeend":
                    return isAscending ? slots.OrderBy(slot => slot.TimeEnd).ToList() : slots.OrderByDescending(slot => slot.TimeEnd).ToList();
                case "status":
                    return isAscending ? slots.OrderBy(slot => slot.Status).ToList() : slots.OrderByDescending(slot => slot.Status).ToList();
                case "roomnumber":
                    return isAscending ? slots.OrderBy(slot => slot.Room.RoomNumber).ToList() : slots.OrderByDescending(slot => slot.Room.RoomNumber).ToList();
            }
            return slots;
        }

        public async Task<ResponseDTO> getAllSlotsOfClinic(string clinicId, int pageNumber, int rowsPerPage, string sortField, string sortOrder)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo.GetAllSlotsOfClinic(clinicId, pageNumber, rowsPerPage);
                List<DentistSlotDTO> dentistSlotDTOList = mapper.Map<List<DentistSlotDTO>>(dentistSlotList);
                dentistSlotDTOList = SortDentistSlots(dentistSlotDTOList, sortField, sortOrder);

                responseDTO.Message = "Get all slots of dentistSlot successfully!";
                responseDTO.Result = dentistSlotDTOList;
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

        public async Task<ResponseDTO> GetAllSlotsOfDentistByTimeStart(string clinicId, DateTime timeStart, DateTime timeEnd)
        {
            ResponseDTO responseDTO = new ResponseDTO("",200,true,null);
            try
            {

                if (clinicId.IsNullOrEmpty())
                {
                    return AddError("Clinic Id is null!", 400);
                }

                Clinic? clinic = await unitOfWork.clinicRepo.getClinicById(clinicId);
                if (clinic == null)
                {
                    return AddError("Clinic is not exist!", 400);
                }

                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo
                    .GetAllDentistSlotsByDentistAndTimeStart(clinicId, timeStart, timeEnd);

                List <DentistSlot> dentistSlots = new List<DentistSlot>();  

                if (dentistSlotList.Count > 0)
                {
                    foreach (var dl in dentistSlotList)
                    {
                        bool check = true;
                        var examinations = dl.Examinations.ToList();
                        if (examinations.Count > 0)
                        {
                            foreach (var e in examinations)
                            {
                                if ((timeStart >= e.TimeStart && timeStart < e.TimeEnd) ||
                                    (timeStart < e.TimeStart && timeEnd > e.TimeStart))
                                {
                                    check = false;
                                    break;
                                }
                            }
                        }
                        if (check == true)
                        {
                            dentistSlots.Add(dl);
                        }
                    }
                }

                List<DentistAndSlotDTO> dentistSlotDTos = mapper.Map<List<DentistAndSlotDTO>>(dentistSlots);
                
                /*foreach (var dl in dentistSlots)
                {
                    UserDTO dentist = mapper.Map<UserDTO>(dl.Dentist);

                    DentistAndSlotDTO dentistAndSlot = new DentistAndSlotDTO();
                    dentistAndSlot.Dentist = mapper.Map<UserDTO>(dl.Dentist);
                    dentistAndSlot.DentistSlotId = dl.DentistSlotId;
                    dentistAndSlots.Add(dentistAndSlot);
                }*/

                responseDTO.Result = dentistSlotDTos;
                
            }catch (Exception e)
            {
                responseDTO = AddError(e.Message, 500);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetAllDentistSlotsByDentistAndDate(string clinicId, string dentistId, DateOnly selectedDate)
        {
            ResponseDTO responseDTO = new ResponseDTO("",200,true,null);
            try
            {
                if (clinicId.IsNullOrEmpty())
                {
                    return AddError("Clinic Id is null!", 400);
                }

                Clinic? clinic = await unitOfWork.clinicRepo.getClinicById(clinicId);
                if (clinic == null)
                {
                    return AddError("Clinic is not found!",404);
                }

                if (dentistId.IsNullOrEmpty())
                {
                    return AddError("Dentist Id is null!", 400);
                }

                User? dentist = await unitOfWork.userRepo.GetByIdAsync(Guid.Parse(dentistId));
                if (dentist == null)
                {
                    return AddError("Dentist is not found!", 404);
                }

                DateOnly minDate = new DateOnly(1990,01,01);
                DateOnly maxDate = selectedDate.AddYears(1);

                if (selectedDate < minDate || selectedDate > maxDate)
                {
                    return AddError("Selected date is out of range!", 400);
                }

                List<DentistSlot>? dentistSlots = await unitOfWork.dentistSlotRepo.GetAllSlotsOfDentistByDate(clinicId, dentistId, selectedDate);

                List<DentistSlotDTO> dentistslotDTOs = mapper.Map<List<DentistSlotDTO>>(dentistSlots);

                responseDTO.Result = dentistslotDTOs;

            }catch (Exception ex)
            {
                AddError(ex.Message, 500);
            }
            return responseDTO;
        }


        private ResponseDTO AddError(string message, int statusCode)
        {
            ResponseDTO responseDTO = new ResponseDTO(message, statusCode, false, null);
            return responseDTO;
        }
    
    }
}
