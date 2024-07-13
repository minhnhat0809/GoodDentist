using BusinessObject.DTO.ViewDTO;

namespace BusinessObject.DTO
{
    public class OrderServiceDTO
    {
        public int OrderServiceId { get; set; }
    
        public int? OrderId { get; set; }
    
        public int? ServiceId { get; set; }
    
        public decimal? Price { get; set; }
    
        public int? Quantity { get; set; }
    
        public int? Status { get; set; }
    
        public virtual OrderDTO? Order { get; set; }
    
        public virtual ICollection<PaymentCreateDTO> Payments { get; set; } = new List<PaymentCreateDTO>();
    
        public virtual ServiceDTO? Service { get; set; }
    }
    public class OrderServiceCreateDTO
    {
        public int OrderServiceId { get; set; }

        public int? OrderId { get; set; }

        public int? ServiceId { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? Status { get; set; }
    }
}

