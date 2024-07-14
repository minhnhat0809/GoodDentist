using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.DentistSlotDTOs
{
    public class UpdateDentistSlotDTO
    {
        public int DentistSlotId { get; set; }

        public Guid? DentistId { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public bool? Status { get; set; }

        public int? RoomId { get; set; }
        
        public required string ClinicId { get; set; }
    }
}
