using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string? OrderName { get; set; }

    public int? ExaminationProfileId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ExaminationProfile? ExaminationProfile { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
