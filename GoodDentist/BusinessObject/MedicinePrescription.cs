using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class MedicinePrescription
{
    public int MedicinePrescriptionId { get; set; }

    public int? MedicineId { get; set; }

    public int? PrescriptionId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Prescription? Prescription { get; set; }
}
