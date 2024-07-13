using BusinessObject.DTO.PaymentDTOs.View;
using BusinessObject.DTO.ServiceDTOs.View;
using BusinessObject.Entity;

namespace BusinessObject.DTO.OrderServiceDTOs;

public class OrderServiceCreateDTO
{
    public int OrderServiceId { get; set; }

    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? Status { get; set; }
}