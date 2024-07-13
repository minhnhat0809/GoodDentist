using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.MedicineDTOs.View;
using BusinessObject.Entity;

namespace BusinessObject.DTO.PrescriptionDTOs
{
    public class PrescriptionCreateDTO
    {
    
        public DateTime? DateTime { get; set; }

        public string? Note { get; set; }

        public bool? Status { get; set; }

        public decimal? Total { get; set; }

        public int? ExaminationId { get; set; }

        public ICollection<MedicineDTO>? Medicines { get; set; } = new List<MedicineDTO>();
    }
}
