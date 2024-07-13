using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.UserDTOs.View;

namespace BusinessObject.DTO.DentistSlotDTOs.View
{
    public class DentistAndSlotDTO
    {
        public UserDTO? Dentist { get; set; }
        public int DentistSlotId { get; set; }
    }
}
