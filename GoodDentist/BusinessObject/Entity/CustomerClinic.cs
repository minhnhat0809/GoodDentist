using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class CustomerClinic
{
    public int CustomerClinicId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? ClinicId { get; set; }

    public bool? Status { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual Customer? Customer { get; set; }
}
