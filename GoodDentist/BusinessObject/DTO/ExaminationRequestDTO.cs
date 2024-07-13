using BusinessObject.Entity;

namespace BusinessObject.DTO.ViewDTO;

public class ExaminationRequestDTO
{
    public int? ExaminationId { get; set; }

    public int? ExaminationProfileId { get; set; }

    public int? DentistSlotId { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public string? Notes { get; set; }

    public int? Status { get; set; }

}