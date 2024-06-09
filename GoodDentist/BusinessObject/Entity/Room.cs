using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomNumber { get; set; }

    public Guid? ClinicId { get; set; }

    public bool? Status { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual ICollection<DentistSlot> DentistSlots { get; set; } = new List<DentistSlot>();
}
