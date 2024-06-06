using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class ClinicService 
{
    public int ClinicServiceId { get; set; }

    public Guid? ClinicId { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual Service? Service { get; set; }
}
