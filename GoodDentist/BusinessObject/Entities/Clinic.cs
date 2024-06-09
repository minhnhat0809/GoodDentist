using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Clinic
{
    public Guid ClinicId { get; set; }

    public string ClinicName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ClinicService> ClinicServices { get; set; } = new List<ClinicService>();

    public virtual ICollection<ClinicUser> ClinicUsers { get; set; } = new List<ClinicUser>();

    public virtual ICollection<CustomerClinic> CustomerClinics { get; set; } = new List<CustomerClinic>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
