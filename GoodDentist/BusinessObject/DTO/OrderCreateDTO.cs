using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
	public class OrderCreateDTO
	{
		public string? OrderName { get; set; }

		public int? ExaminationId { get; set; }

		public DateTime? DateTime { get; set; }

		public decimal? Price { get; set; }

		public bool? Status { get; set; }
		
		public int? PaymentId { get; set; }

		//public ICollection<OrderServiceCreateDTO>? OrderServices { get; set; } = new List<OrderServiceCreateDTO>();
		public ICollection<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
	}
}
