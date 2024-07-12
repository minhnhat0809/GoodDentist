using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ViewDTO
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
        
    }
}
