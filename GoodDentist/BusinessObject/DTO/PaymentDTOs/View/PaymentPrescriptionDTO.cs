using BusinessObject.DTO.PrescriptionDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs.View;

public class PaymentPrescriptionDTO
{
    public int PaymentPrescriptionId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Price { get; set; }

    public int? PrescriptionId { get; set; }

    public bool? Status { get; set; }

    public virtual PrescriptionDTO? Prescription { get; set; }
}