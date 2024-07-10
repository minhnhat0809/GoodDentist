using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ViewDTO
{
    public class DentistAndSlotDTO
    {
        public UserDTO? Dentist { get; set; }
        public int DentistSlotId { get; set; }
    }
}
