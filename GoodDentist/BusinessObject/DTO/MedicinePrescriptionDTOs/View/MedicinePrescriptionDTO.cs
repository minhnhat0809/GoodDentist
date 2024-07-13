using BusinessObject.DTO.MedicineDTOs.View;

namespace BusinessObject.DTO.MedicinePrescriptionDTOs.View;

public class MedicinePrescriptionDTO
{
    public int MedicinePrescriptionId { get; set; }

    public int? MedicineId { get; set; }

    public int? PrescriptionId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public MedicineDTO? Medicine { get; set; }
}