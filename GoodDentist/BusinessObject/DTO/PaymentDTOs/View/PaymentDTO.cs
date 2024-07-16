using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.OrderServiceDTOs.View;
using BusinessObject.DTO.ServiceDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs.View
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }

        public string? PaymentDetail { get; set; }

        public DateTime? CreateAt { get; set; }

        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public virtual ServiceToOrderDTO? Order { get; set; }
    }
}

