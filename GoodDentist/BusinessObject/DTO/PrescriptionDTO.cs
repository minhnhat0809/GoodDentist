﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entity;

namespace BusinessObject.DTO
{
	public class PrescriptionDTO
	{
		public int PrescriptionId { get; set; }

		public DateTime? DateTime { get; set; }

		public string? Note { get; set; }

		public decimal? Total { get; set; }

		public int? ExaminationId { get; set; }
		}
}