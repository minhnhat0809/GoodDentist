using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public interface INotificationService
    {
        Task<NotificationDTO> SendSingleNotificationAsync(NotificationRequestDTO notification);
        Task<List<NotificationDTO>> GetUserNotificationsAsync(Guid userId);
        NotificationDTO SendUsersNotificationAsync(List<User> users, NotificationRequestDTO notification);
        Task<NotificationDTO> DeleteNotificationAsync(int notificationId);
        Task<NotificationDTO> UpdateNotificationAsync(NotificationDTO notification);
    }
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<NotificationDTO>> GetUserNotificationsAsync(Guid userId)
        {

            try
            {
                var notifications = await _unitOfWork.NotificationRepository.GetUserNotificationsAsync(userId);
                var viewModels = _mapper.Map<List<NotificationDTO>>(notifications);   
                return viewModels;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve notifications.", ex);
            }
        }

        public async Task<NotificationDTO> SendSingleNotificationAsync(NotificationRequestDTO notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            var model = _mapper.Map<Notification>(notification); 
            try
            {
                var userModel = await _unitOfWork.userRepo.GetByIdAsync(notification.UserId);
                if (notification.UserId == Guid.Empty)
                {
                    // send all
                    model.IsPublic = true;
                    _unitOfWork.NotificationRepository.CreateNotificationAsync(model);
                    var viewPublicModel = _mapper.Map<NotificationDTO>(model);
                    return viewPublicModel;
                }
                // single pipo
                model.Users.Add(userModel);
                model.IsPublic = false;
                _unitOfWork.NotificationRepository.CreateNotificationAsync(model);
                var viewModel = _mapper.Map<NotificationDTO>(model);  
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send notification.", ex);
            }
        }

        public NotificationDTO SendUsersNotificationAsync(List<User> users,NotificationRequestDTO notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            var model = _mapper.Map<Notification>(notification);
            try
            {
                // send all user
                model.Users = users;
                model.IsPublic = false;
                _unitOfWork.NotificationRepository.CreateNotificationAsync(model);
                var viewModel = _mapper.Map<NotificationDTO>(model);
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send notification.", ex);
            }
        }

        public async Task<NotificationDTO> UpdateNotificationAsync(NotificationDTO notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            try
            {
                var existingNotification = await _unitOfWork.NotificationRepository.GetNotificationAsync(notification.NotificationId);
                if (existingNotification == null)
                {
                    throw new Exception("Notification not found.");
                }

                _mapper.Map(notification, existingNotification);

                _unitOfWork.NotificationRepository.UpdateNotificationAsync(existingNotification);

                var updatedNotificationDTO = _mapper.Map<NotificationDTO>(existingNotification);
                return updatedNotificationDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update notification.", ex);
            }
        }
        
        public async Task<NotificationDTO> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _unitOfWork.NotificationRepository.GetNotificationAsync(notificationId);
                if (notification == null)
                {
                    throw new Exception("Not Found notification.");
                }

                _unitOfWork.NotificationRepository.DeleteNotificationAsync(notificationId);
                return _mapper.Map<NotificationDTO>(notification);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete notification.", ex);
            }
        }
    }
}
