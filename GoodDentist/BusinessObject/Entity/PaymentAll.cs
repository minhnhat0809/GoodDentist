using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class PaymentAll
{
    public int PaymentAllId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public int? PaymentPrescriptionId { get; set; }

    public bool? Status { get; set; }

    public virtual PaymentPrescription? PaymentPrescription { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
