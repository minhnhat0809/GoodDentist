using BusinessObject.DTO.ServiceDTOs.View;

namespace BusinessObject.DTO.OrderDTOs;

public class OrderUpdateDTO
{
    public int OrderId { get; set; }
    public string? OrderName { get; set; }

    public int? ExaminationId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }

    public ICollection<ServiceToOrderDTO> Services { get; set; } = new List<ServiceToOrderDTO>();
}