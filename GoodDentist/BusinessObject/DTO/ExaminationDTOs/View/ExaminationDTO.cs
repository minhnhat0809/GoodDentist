using BusinessObject.DTO.DentistSlotDTOs.View;
using BusinessObject.DTO.MedicalRecordDTOs.View;
using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs.View;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ExaminationDTOs.View
{
    public class ExaminationDTO
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

        public DentistSlotForExamDTO? DentistSlot { get; set; }

        public ExaminationProfileForExamDTO? ExaminationProfile { get; set; }

        public ICollection<OrderDTO>? Orders { get; set; }

        public ICollection<PrescriptionDTO>? Prescriptions { get; set; }

        public ICollection<MedicalRecordDTO>? MedicalRecords { get; set; }



    }
}
