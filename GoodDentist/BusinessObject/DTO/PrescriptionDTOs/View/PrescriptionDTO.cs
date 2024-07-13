using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.MedicinePrescriptionDTOs.View;
using BusinessObject.Entity;

namespace BusinessObject.DTO.PrescriptionDTOs.View
{
    public class PrescriptionDTO
    {
        public int PrescriptionId { get; set; }

        public DateTime? DateTime { get; set; }

        public string? Note { get; set; }

        public decimal? Total { get; set; }

        public int? ExaminationId { get; set; }

        public bool? Status { get; set; }

        public ICollection<MedicinePrescriptionDTO>? MedicinePrescriptions { get; set; }

    }
}
