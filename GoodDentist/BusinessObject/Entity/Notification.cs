using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string NotificationTitle { get; set; } = null!;

    public string NotificationMessage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsPublic { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
