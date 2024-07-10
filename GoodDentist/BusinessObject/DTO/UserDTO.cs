﻿using System;
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
        public required string UserName { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public required int RoleId { get; set; }
        public string? Avatar { get; set; }
        public ICollection<ClinicDTO>? Clinics { get; set; }
    }

    
}
