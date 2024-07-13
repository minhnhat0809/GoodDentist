using BusinessObject.DTO.UserDTOs.View;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.NotificationDTOs.View
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }

        public string NotificationTitle { get; set; } = null!;

        public string NotificationMessage { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsPublic { get; set; }

        public virtual ICollection<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
