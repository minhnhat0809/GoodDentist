using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ViewDTO
{
    public class RoomDTO
    {
        public int RoomId { get; set; }

        public string? RoomNumber { get; set; }

        public Guid? ClinicId { get; set; }

        public bool? Status { get; set; }
    }
}
