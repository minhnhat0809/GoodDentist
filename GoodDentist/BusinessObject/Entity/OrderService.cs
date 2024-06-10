using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class OrderService
{
    public int OrderServiceId { get; set; }

    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public bool? Status { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Service? Service { get; set; }
}
