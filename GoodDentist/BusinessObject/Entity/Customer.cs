using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Customer
{
    public Guid CustomerId { get; set; }

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public byte[]? Salt { get; set; }

    public byte[]? Password { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<CustomerClinic> CustomerClinics { get; set; } = new List<CustomerClinic>();

    public virtual ICollection<ExaminationProfile> ExaminationProfiles { get; set; } = new List<ExaminationProfile>();
}
