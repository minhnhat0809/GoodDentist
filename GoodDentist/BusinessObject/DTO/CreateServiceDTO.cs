using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
	public partial class CreateServiceDTO
	{
		public int ServiceId { get; set; }
		public string ServiceName { get; set; } = null!;

		public string? Description { get; set; }

		public decimal? Price { get; set; }

		public bool? Status { get; set; }
	}
}
