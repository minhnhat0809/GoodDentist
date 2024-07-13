using BusinessObject.DTO.ServiceDTOs.View;

namespace BusinessObject.DTO.OrderServiceDTOs.View;

public class OrderServiceDTO
{
    public int OrderServiceId { get; set; }

    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? Status { get; set; }

    public ServiceDTO? Service { get; set; }
}