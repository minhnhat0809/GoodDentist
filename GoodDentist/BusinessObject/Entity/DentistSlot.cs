using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class DentistSlot
{
    public int DentistSlotId { get; set; }

    public Guid? DentistId { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string? Available { get; set; }

    public int? RoomId { get; set; }

    public bool? Status { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public virtual Room? Room { get; set; }
}
