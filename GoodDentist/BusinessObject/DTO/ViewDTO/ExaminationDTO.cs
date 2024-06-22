using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ViewDTO
{
    public class ExaminationDTO
    {
        public int ExaminationId { get; set; }

        public int? ExaminationProfileId { get; set; }

        public Guid? DentistId { get; set; }

        public string? DentistName {  get; set; }

        public int? DentistSlotId { get; set; }

        public string? Diagnosis { get; set; }

        public DateTime? TimeStart { get; set; }

        public TimeOnly? Duration { get; set; }

        public string? Notes { get; set; }

        public bool? Status { get; set; }
    }
}
