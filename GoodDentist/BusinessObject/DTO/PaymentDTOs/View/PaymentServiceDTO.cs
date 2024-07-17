namespace BusinessObject.DTO.PaymentDTOs.View;

public class PaymentServiceDTO
{
    public string ServiceName { get; set; }
    
    public decimal Total { get; set; }

    public PaymentServiceDTO(string serviceName, decimal total)
    {
        this.ServiceName = serviceName;
        this.Total = total;
    }
}