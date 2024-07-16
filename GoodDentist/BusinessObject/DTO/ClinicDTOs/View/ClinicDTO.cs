using BusinessObject.DTO.ClinicServiceDTOs.View;
using BusinessObject.DTO.RoomDTOs.View;

namespace BusinessObject.DTO.ClinicDTOs.View;

public class ClinicDTO
{
    public Guid ClinicId { get; set; }

    public string ClinicName { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }
    
    public virtual ICollection<ClinicServiceDTO> ClinicServices { get; set; } = new List<ClinicServiceDTO>();

    public virtual ICollection<RoomDTO> Rooms { get; set; } = new List<RoomDTO>();

}