namespace BusinessObject.DTO.ViewDTO;

public class DentistSlotDTO
{
    public int DentistSlotId { get; set; }

    public Guid? DentistId { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public bool? Status { get; set; }

    public int? RoomId { get; set; }

    public virtual User? Dentist { get; set; }
    
    public int RoomNumber { get; set; }

}