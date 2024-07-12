using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CreateUserDTO
    {
        public string? UserName { get; set;}
        public string? Password { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob {  get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public required string ClinicId { get; set; }
        public required int RoleId { get; set; }

        public IFormFile? Avatar { get; set; }
    }
    
}
