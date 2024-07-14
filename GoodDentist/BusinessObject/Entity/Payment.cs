using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? PaymentDetail { get; set; }

    public DateTime? CreateAt { get; set; }

    public decimal? Price { get; set; }

    public int? OrderServiceId { get; set; }

    public int? PaymentAllId { get; set; }

    public bool? Status { get; set; }

    public virtual OrderService? OrderService { get; set; }

    public virtual PaymentAll? PaymentAll { get; set; }
}
