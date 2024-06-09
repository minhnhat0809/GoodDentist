using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class MedicalRecord
{
    public int MedicalRecordId { get; set; }

    public int? ExaminationId { get; set; }

    public int? RecordTypeId { get; set; }

    public string? Url { get; set; }

    public string? Notes { get; set; }

    public bool? Status { get; set; }

    public virtual Examination? Examination { get; set; }

    public virtual RecordType? RecordType { get; set; }
}
