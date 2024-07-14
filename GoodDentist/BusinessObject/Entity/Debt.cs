using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Debt
{
    public int TotalId { get; set; }

    public decimal? Total { get; set; }

    public int? ExaminationProfileId { get; set; }
}
