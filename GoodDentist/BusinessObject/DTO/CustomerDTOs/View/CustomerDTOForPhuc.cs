using BusinessObject.DTO.ClinicDTOs.View;
using BusinessObject.DTO.ExaminationProfileDTOs.View;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.CustomerDTOs.View
{
    public class CustomerDTOForPhuc
    {
        public Guid CustomerId { get; set; }

        public string UserName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly? Dob { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Address { get; set; }

        public string? Anamnesis { get; set; }

        public string? Avatar { get; set; }

        public bool? Status { get; set; }

        public ICollection<ClinicDTO> Clinics { get; set; } = new List<ClinicDTO>();

        public ICollection<ExaminationProfileForCustomerDTO> ExaminationProfiles { get; set; } = new List<ExaminationProfileForCustomerDTO>();
    }
}
