using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class RecordType
{
    public int RecordTypeId { get; set; }

    public string RecordName { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
