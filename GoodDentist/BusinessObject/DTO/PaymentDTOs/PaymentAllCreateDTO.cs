using BusinessObject.DTO.PaymentDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs;

public class PaymentAllCreateDTO
{
    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public int? PaymentId { get; set; }

    public int? PaymentPrescriptionId { get; set; }

    public bool? Status { get; set; }
}