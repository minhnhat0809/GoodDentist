using System;
using System.Collections.Generic;

namespace BusinessObject.Entity;

public partial class Examination
{
    public int ExaminationId { get; set; }

    public int? ExaminationProfileId { get; set; }

    public Guid? DentistId { get; set; }

    public int? DentistSlotId { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public string? Notes { get; set; }

    public int? Status { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual DentistSlot? DentistSlot { get; set; }

    public virtual ExaminationProfile? ExaminationProfile { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
