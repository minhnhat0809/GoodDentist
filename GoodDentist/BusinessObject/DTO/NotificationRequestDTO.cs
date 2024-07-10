using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class NotificationRequestDTO
    {
        public string NotificationTitle { get; set; } = null!;

        public string NotificationMessage { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }    

    }
}
