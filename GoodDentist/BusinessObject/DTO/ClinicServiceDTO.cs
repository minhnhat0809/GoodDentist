using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
	public class ClinicServiceDTO
	{
		public int ClinicServiceId { get; set; }

		public Guid? ClinicId { get; set; }

		public int? ServiceId { get; set; }

		public decimal? Price { get; set; }

		public bool? Status { get; set; }

	}
}
