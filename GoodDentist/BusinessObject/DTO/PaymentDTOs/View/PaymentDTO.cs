using BusinessObject.DTO.OrderServiceDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs.View
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
    
        public string? PaymentDetail { get; set; }
    
        public decimal? Price { get; set; }
    
        public int? OrderServiceId { get; set; }
    
        public bool? Status { get; set; }
    
        public virtual OrderServiceDTO? OrderService { get; set; }
    }
}

