﻿using BusinessObject.Entity;

namespace BusinessObject.DTO.ViewDTO;

public class ExaminationProfileDTO
{
    public int ExaminationProfileId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? DentistId { get; set; }

    public DateOnly? Date { get; set; }

    public string? Diagnosis { get; set; }

    public bool? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual User? Dentist { get; set; }

    public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();
}