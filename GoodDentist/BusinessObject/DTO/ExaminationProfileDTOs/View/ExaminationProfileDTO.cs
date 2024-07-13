using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ExaminationDTOs.View;
using BusinessObject.DTO.UserDTOs.View;

namespace BusinessObject.DTO.ExaminationProfileDTOs.View
{
    public class ExaminationProfileDTO
    {
        public int ExaminationProfileId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? DentistId { get; set; }

        public DateOnly? Date { get; set; }

        public string? Diagnosis { get; set; }

        public bool? Status { get; set; }

        public UserDTO? Customer { get; set; }

        public UserDTO? Dentist { get; set; }

        public List<ExaminationDTO>? Examinations { get; set; }
    }
}
