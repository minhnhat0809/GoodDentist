using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Price { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
