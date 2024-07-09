using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
        Task<Notification> GetNotificationAsync(int notificationId);
        Notification CreateNotificationAsync(Notification notification);
        Notification UpdateNotificationAsync(Notification notification);
        Notification DeleteNotificationAsync(int notificationId);

    }
    public class NotificationRepository : INotificationRepository
    {
        private readonly GoodDentistDbContext _context;
        public NotificationRepository(GoodDentistDbContext context)
        {
            _context = context;
        }
        Notification INotificationRepository.UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return notification;
        }

        public  Notification DeleteNotificationAsync(int notificationId)
        {
            var model =  _context.Notifications
                .Include(x=>x.Users)
                .FirstOrDefaultAsync(x=>x.NotificationId == notificationId).Result;
            model.Users.Clear();
            _context.Notifications.Remove(model);
            _context.SaveChanges();
            return model;
        }

        public Notification CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }


        public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                // If userId is empty, return public notifications
                return await _context.Notifications
                    .Where(n => n.IsPublic)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();
            }
            else
            {
                // Get notifications for the specified user
                var notifications = await _context.Notifications
                    .Include(n => n.Users)
                    .Where(n => n.IsPublic || n.Users.Any(u => u.UserId == userId))
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                return notifications;
            }
        }


        public async Task<Notification> GetNotificationAsync(int notificationId)
        {
            var model = await _context.Notifications.FindAsync(notificationId);
            return model;
        }

        
    }
}
