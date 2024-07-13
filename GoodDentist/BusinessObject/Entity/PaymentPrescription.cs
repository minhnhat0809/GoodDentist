using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class PaymentPrescription
{
    public int PaymentPrescriptionId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Price { get; set; }

    public int? PrescriptionId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<PaymentAll> PaymentAlls { get; set; } = new List<PaymentAll>();

    public virtual Prescription? Prescription { get; set; }
}
