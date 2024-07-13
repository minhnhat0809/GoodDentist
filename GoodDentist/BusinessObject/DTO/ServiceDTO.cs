namespace BusinessObject.DTO;

public class ServiceDTO
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? Status { get; set; }
}