using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
	public class OrderDTO
	{
		public int OrderId { get; set; }

		public string? OrderName { get; set; }

		public int? ExaminationId { get; set; }

		public DateTime? DateTime { get; set; }

		public decimal? Price { get; set; }
	}
}
