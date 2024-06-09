using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Price { get; set; }

    public int? OrderId { get; set; }

    public bool? Status { get; set; }

    public virtual Order? Order { get; set; }
}
