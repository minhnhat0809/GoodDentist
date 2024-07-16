namespace BusinessObject.DTO.PaymentDTOs.View;

public class PaymentAllDTO
{
    public int PaymentAllId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public bool? Status { get; set; }
    
    public virtual PaymentDTO? PaymentOrder { get; set; }

    public virtual PaymentPrescriptionDTO? PaymentPrescription { get; set; }
}