using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTO.CustomerDTOs;

public class CustomerUpdateRequestDTO
{
    public required string CustomerId { get; set; }
    
    public string Name { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Anamnesis { get; set; }

    public string ClinicId { get; set; } = null!;

    public bool? Status { get; set; }

    public IFormFile? Avatar { get; set; }
}