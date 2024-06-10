using BusinessObject.Entity;

namespace BusinessObject.DTO;

public class MedicalRecordRequestDTO
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