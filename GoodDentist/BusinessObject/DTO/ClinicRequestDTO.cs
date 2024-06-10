namespace BusinessObject.DTO.ViewDTO;

public class ClinicRequestDTO
{
    public Guid ClinicId { get; set; }

    public string ClinicName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }
}