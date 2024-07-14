﻿using BusinessObject.DTO.OrderServiceDTOs.View;

namespace BusinessObject.DTO.PaymentDTOs.View
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }

        public string? PaymentDetail { get; set; }

        public DateTime? CreateAt { get; set; }

        public decimal? Price { get; set; }

        public int? OrderServiceId { get; set; }

        public int? PaymentAllId { get; set; }

        public bool? Status { get; set; }
    }
}

