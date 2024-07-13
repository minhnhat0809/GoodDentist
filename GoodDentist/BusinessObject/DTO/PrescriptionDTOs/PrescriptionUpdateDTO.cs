using BusinessObject.DTO.MedicineDTOs.View;

namespace BusinessObject.DTO.PrescriptionDTOs;

public class PrescriptionUpdateDTO
{
    public int PrescriptionId { get; set; }
    
    public DateTime? DateTime { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    public decimal? Total { get; set; }

    public int? ExaminationId { get; set; }

    public List<MedicineDTO>? Medicines { get; set; } = new List<MedicineDTO>();
}