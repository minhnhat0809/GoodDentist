using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class User
{
    public Guid UserId { get; set; }

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

    public int? RoleId { get; set; }

    public bool? Status { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<ClinicUser> ClinicUsers { get; set; } = new List<ClinicUser>();

    public virtual ICollection<DentistSlot> DentistSlots { get; set; } = new List<DentistSlot>();

    public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    public virtual Role? Role { get; set; }
}
