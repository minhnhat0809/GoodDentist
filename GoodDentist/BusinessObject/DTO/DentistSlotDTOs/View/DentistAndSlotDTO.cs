using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ExaminationDTOs.View;
using BusinessObject.DTO.RoomDTOs.View;
using BusinessObject.DTO.UserDTOs.View;

namespace BusinessObject.DTO.DentistSlotDTOs.View
{
    public class DentistAndSlotDTO
    {
        public int DentistSlotId { get; set; }

        public Guid? DentistId { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public int? RoomId { get; set; }

        public bool? Status { get; set; }

        public UserForExamDTO? Dentist { get; set; }

        public ICollection<ExaminationForDentistSlotDTO> Examinations { get; set; } = new List<ExaminationForDentistSlotDTO>();

        public RoomDTO? Room { get; set; }
    }
}
