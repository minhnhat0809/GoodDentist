using BusinessObject.Entity;

namespace BusinessObject.DTO.ViewDTO;

public class ExaminationRequestDTO
{
    public int ExaminationId { get; set; }

    public int? ExaminationProfileId { get; set; }

    public Guid? DentistId { get; set; }

    public int? DentistSlotId { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime? TimeStart { get; set; }

    public TimeOnly? Duration { get; set; }

    public string? Notes { get; set; }

    public bool? Status { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual DentistSlot? DentistSlot { get; set; }

    public virtual ExaminationProfile? ExaminationProfile { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

}