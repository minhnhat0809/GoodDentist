using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ClinicUser
{
    public int ClinicUserId { get; set; }

    public Guid? ClinicId { get; set; }

    public Guid? UserId { get; set; }

    public bool? Status { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual User? User { get; set; }
}
