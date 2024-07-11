using BusinessObject.DTO.ViewDTO;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Impl;
using BusinessObject.Entity;

namespace GoodDentist.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        public ResponseDTO _responseDto;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ResponseDTO> GetUserNotificationsAsync(
            [FromQuery] Guid userId)

        {
            _responseDto = new ResponseDTO("Get Successfully", 200, true, null);
            try
            {
                var models = await _service.GetUserNotificationsAsync(userId);
                _responseDto.Result = models;

            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
        
        [HttpPost]
        public async Task<ActionResult<ResponseDTO>> SendNotificationAsync([FromBody] NotificationRequestDTO notification)
        {
            _responseDto = new ResponseDTO("", 200, true, null);
            try
            {
                var createdClinic = await _service.SendSingleNotificationAsync(notification);
                _responseDto.Result = createdClinic;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDTO>> DeleteNotificationAsync(int id)
        {
            _responseDto = new ResponseDTO("Delete successfully", 200, true, null);
            try
            {
                var viewModel = await _service.DeleteNotificationAsync(id);
                _responseDto.Result = viewModel;
                return _responseDto;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
       
        [HttpPut]
        public async Task<ActionResult<ResponseDTO>> UpdateNotificationAsync([FromBody] NotificationDTO notification)
        {
            _responseDto = new ResponseDTO("Update Successfully", 200, true, null);
            try
            {
                var updatedNotification = await _service.UpdateNotificationAsync(notification);
                _responseDto.Result = updatedNotification;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
                _responseDto.StatusCode = 500;
            }

            return _responseDto;
        }
    }

}
