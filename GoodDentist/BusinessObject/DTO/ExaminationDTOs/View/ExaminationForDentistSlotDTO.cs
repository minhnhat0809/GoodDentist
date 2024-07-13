using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ExaminationDTOs.View
{
    public class ExaminationForDentistSlotDTO
    {
        public int ExaminationId { get; set; }

        public int? ExaminationProfileId { get; set; }

        public string? DentistId { get; set; }

        public string? DentistName { get; set; }

        public int? DentistSlotId { get; set; }

        public string? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? Diagnosis { get; set; }

        public required DateTime TimeStart { get; set; }

        public required DateTime TimeEnd { get; set; }

        public string? Notes { get; set; }

        public int? Status { get; set; }
    }
}
