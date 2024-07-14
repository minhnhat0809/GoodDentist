using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs;

public class PaymentAllCreateDTO
{
    public string? PaymentDetail { get; set; }

    public decimal? Total { get; set; }

    public bool? Status { get; set; }

    public PrescriptionDTO? Prescription { get; set; }

    public OrderDTO? Order { get; set; }

}