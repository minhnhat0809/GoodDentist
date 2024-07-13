using BusinessObject.Entity;

namespace BusinessObject.DTO.ViewDTO;

public class MedicalRecordDTO
{
    public int MedicalRecordId { get; set; }

    public int? ExaminationId { get; set; }

    public int? RecordTypeId { get; set; }

    public string? Url { get; set; }

    public string? Notes { get; set; }

    public bool? Status { get; set; }

    public string? RecordType { get; set; }
}