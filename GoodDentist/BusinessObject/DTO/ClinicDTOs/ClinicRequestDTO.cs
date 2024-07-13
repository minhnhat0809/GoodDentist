namespace BusinessObject.DTO.ClinicDTOs;

public class ClinicRequestDTO
{
    public Guid ClinicId { get; set; }

    public string ClinicName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }
}