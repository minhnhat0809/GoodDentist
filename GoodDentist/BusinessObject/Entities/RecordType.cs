using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class RecordType
{
    public int RecordTypeId { get; set; }

    public string RecordName { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
