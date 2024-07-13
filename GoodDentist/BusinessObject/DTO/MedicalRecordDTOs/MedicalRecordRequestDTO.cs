using BusinessObject.Entity;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTO.MedicalRecordDTOs
{

    public class MedicalRecordRequestDTO
    {
        public int MedicalRecordId { get; set; }

        public int? ExaminationId { get; set; }

        public int? RecordTypeId { get; set; }

        public string? Url { get; set; }

        public string? Notes { get; set; }

        public bool? Status { get; set; }

        public virtual Examination? Examination { get; set; }

        public virtual RecordType? RecordType { get; set; }
    }
    public class MedicalRecordRequestTestDTO
    {
        public int MedicalRecordId { get; set; }

        public int? ExaminationId { get; set; }

        public int? RecordTypeId { get; set; }

        public IFormFile? UploadFile { get; set; }

        public string? Notes { get; set; }

        public bool? Status { get; set; }
    }
}
