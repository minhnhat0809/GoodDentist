using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Medicine
{
    public int MedicineId { get; set; }

    public string MedicineName { get; set; } = null!;

    public string? Type { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }
}
