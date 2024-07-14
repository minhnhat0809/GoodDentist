using BusinessObject.DTO.OrderDTOs;
using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs;
using BusinessObject.DTO.PrescriptionDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs;

public class PaymentAllUpdateDTO
{
    public int PaymentAllId { get; set; }
    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public bool? Status { get; set; }

    public PrescriptionUpdateDTO? Prescription { get; set; }

    public OrderUpdateDTO? Order { get; set; }
    
}