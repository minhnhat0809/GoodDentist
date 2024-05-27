using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public DateTime? DateTime { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    public decimal? Total { get; set; }

    public int? ExaminationId { get; set; }

    public virtual Examination? Examination { get; set; }

    public virtual ICollection<MedicinePrescription> MedicinePrescriptions { get; set; } = new List<MedicinePrescription>();
}
