namespace BusinessObject.DTO.PaymentDTOs.View;

public class PaymentAllDTO
{
    public int PaymentAllId { get; set; }

    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public int? PaymentPrescriptionId { get; set; }

    public bool? Status { get; set; }

    public virtual PaymentPrescriptionDTO? PaymentPrescription { get; set; }

    public virtual ICollection<PaymentDTO> Payments { get; set; } = new List<PaymentDTO>();
}