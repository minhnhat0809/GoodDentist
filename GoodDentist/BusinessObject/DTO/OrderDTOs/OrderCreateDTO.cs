using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ServiceDTOs.View;
using BusinessObject.Entity;

namespace BusinessObject.DTO.OrderDTOs
{
    public class OrderCreateDTO
    {
        public string? OrderName { get; set; }

        public int? ExaminationId { get; set; }

        public DateTime? DateTime { get; set; }

        public decimal? Price { get; set; }

        public bool? Status { get; set; }

        public ICollection<ServiceDTO> Services = new List<ServiceDTO>();
    }
}
