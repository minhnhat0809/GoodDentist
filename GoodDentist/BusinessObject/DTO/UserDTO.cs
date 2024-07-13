using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;
using BusinessObject.Entity;

namespace BusinessObject.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public ICollection<ClinicDTO>? Clinics { get; set; }

        public ICollection<ExaminationProfileDTO>? ExaminationProfiles {  get; set; }
    }

    
}
