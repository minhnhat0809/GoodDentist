namespace BusinessObject.DTO.DentistSlotDTOs;

public class CreateDentistSlotDTO
{
    public Guid? DentistId { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public bool? Status { get; set; }

    public int? RoomId { get; set; }
        
    public required string ClinicId { get; set; }
}