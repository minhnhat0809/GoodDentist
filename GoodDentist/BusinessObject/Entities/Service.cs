using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ClinicService> ClinicServices { get; set; } = new List<ClinicService>();

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
}
