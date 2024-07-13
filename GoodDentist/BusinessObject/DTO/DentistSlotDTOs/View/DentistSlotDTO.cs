﻿using BusinessObject.DTO.ExaminationDTOs.View;
using BusinessObject.DTO.RoomDTOs.View;
using BusinessObject.DTO.UserDTOs.View;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.DentistSlotDTOs.View
{
    public class DentistSlotDTO
    {
        public int DentistSlotId { get; set; }

        public Guid? DentistId { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public int? RoomId { get; set; }

        public bool? Status { get; set; }

        public UserDTO? Dentist { get; set; }

        public ICollection<ExaminationForDentistSlotDTO> Examinations { get; set; } = new List<ExaminationForDentistSlotDTO>();

        public RoomDTO? Room { get; set; }
    }
}
