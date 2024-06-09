using AutoMapper;
using BusinessObject.DTO;
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

        public async Task<ResponseListDTO> createDentistSlot(DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.IsSuccess = true;
            try
            {
                responseDTO = await validateDentistSlot(dentistSlotDTO);
                if (responseDTO.IsSuccess == false)
                {
                    return responseDTO;
                }

                DentistSlot dentistSlot = mapper.Map<DentistSlot>(dentistSlotDTO);
                await unitOfWork.dentistSlotRepo.CreateAsync(dentistSlot);

                responseDTO.Message.Add("Create sucessfully");
                responseDTO.IsSuccess = true;
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
            string filterField, string filterValue, string sortField, string sortOrder)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo.GetAllDentistSlots(pageNumber, rowsPerPage);

                List<DentistSlotDTO> dentistSlotDTOList = mapper.Map<List<DentistSlotDTO>>(dentistSlotList);

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
            string filterField, string filterValue, string sortField, string sortOrder)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            try
            {
                List<DentistSlot>? dentistSlotList = await unitOfWork.dentistSlotRepo.GetAllSlotsOfDentist(dentistId, pageNumber, rowsPerPage);
                List<DentistSlotDTO> dentistSlotDTOList = mapper.Map<List<DentistSlotDTO>>(dentistSlotList);

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
                dentistSlotDTO.RoomNumber = dentistSlot.Room.RoomNumber;

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

        public async Task<ResponseListDTO> updateDentistSlot(DentistSlotDTO dentistSlotDTO)
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

        private async Task<ResponseListDTO> validateDentistSlot(DentistSlotDTO dentistSlotDTO)
        {
            ResponseListDTO responseDTO = new ResponseListDTO();
            responseDTO.IsSuccess = true;
            bool checkTime = true;
            void AddError(string message)
            {
                responseDTO.Message.Add(message);
                responseDTO.IsSuccess = false;
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
            }else if (checkTime)
                {
                    List<string> errors = await checkRoomAvailable((int)dentistSlotDTO.RoomId, (DateTime)dentistSlotDTO.TimeStart, dentistSlotDTO.DentistId.ToString());
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

        private async Task<List<string>> checkRoomAvailable(int roomId, DateTime timeStart, string dentistId)
        {
            List<string> errors = new List<string>();
            Room? room = await unitOfWork.roomRepo.GetByIdAsync(roomId);
            if (room == null)
            {
                errors.Add("Room is not existed !!!");
            }

            DentistSlot? dentistSlot = await unitOfWork.dentistSlotRepo.GetDentistSlotByDentistAndTimeStart(dentistId, timeStart);
            if (dentistSlot != null)
            {
                errors.Add("This dentist already has this slot.");
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


    }
}
