namespace BusinessObject.DTO.MedicinePrescriptionDTOs;

public class MedicinePrescriptionCreateDTO
{
    public int MedicinePrescriptionId { get; set; }

    public int? MedicineId { get; set; }

    public int? PrescriptionId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }
}