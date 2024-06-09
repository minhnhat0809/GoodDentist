using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Customer
{
    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? FrontIdCard { get; set; }

    public string? BackIdCard { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<CustomerClinic> CustomerClinics { get; set; } = new List<CustomerClinic>();

    public virtual ICollection<ExaminationProfile> ExaminationProfiles { get; set; } = new List<ExaminationProfile>();
}
