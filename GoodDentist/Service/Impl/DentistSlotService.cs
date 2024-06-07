using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
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
            }catch (Exception ex)
            {
                responseDTO.Message.Add(ex.Message);
                responseDTO.IsSuccess = false;
                //responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> deleteDentistSlot(int slotId)
        {
            ResponseDTO responseDTO = new ResponseDTO("",200,true,null);
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
            }catch (Exception ex)
            {
                responseDTO.Message = ex.Message;
                responseDTO.IsSuccess = false;
                responseDTO.StatusCode = 500;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO> getAllDentistSlots(int pageNumber, int rowsPerPage)
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

        public async Task<ResponseDTO> getAllSlotsOfDentist(string dentistId, int pageNumber, int rowsPerPage)
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
                    responseDTO.Message.Add("There are no dentistSlot slot with this ID");
                    responseDTO.IsSuccess = false;
                    return responseDTO;
                }

                dentistSlot = mapper.Map<DentistSlot>(dentistSlotDTO);
                await unitOfWork.dentistSlotRepo.UpdateAsync(dentistSlot);

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


            return responseDTO;
        }
    }
}
