using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CreateUserDTO
    {
        public required string UserName { get; set;}
        public required string Password { get; set; }
        public DateTime? Dob {  get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public required string ClinicId { get; set; }
        public required int RoleId { get; set; }
    }
}
