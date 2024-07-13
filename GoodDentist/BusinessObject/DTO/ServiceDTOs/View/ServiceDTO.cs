namespace BusinessObject.DTO.ServiceDTOs.View
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }
    
        public string? ServiceName { get; set; }
    
        public string? Description { get; set; }
    
        public decimal? Price { get; set; }
    
        public bool? Status { get; set; }
    }

    public class ServiceToOrderDTO
    {
        public int ServiceId { get; set; }
    
        public string? ServiceName { get; set; }
    
        public string? Description { get; set; }
    
        public decimal? Price { get; set; }
    
        public bool? Status { get; set; }
        
        public int? Quantity { get; set; } = 1;
    }
}

