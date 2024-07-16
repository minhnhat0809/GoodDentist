using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Order
{
    public int OrderId { get; set; }

    public string? OrderName { get; set; }

    public int? ExaminationId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public virtual Examination? Examination { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();

    public virtual Payment? Payment { get; set; }
}
