using BusinessObject.DTO.ServiceDTOs.View;

namespace BusinessObject.DTO.ClinicDTOs;

public class ClinicCreateDTO
{
    public string ClinicName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }
    public ICollection<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
}