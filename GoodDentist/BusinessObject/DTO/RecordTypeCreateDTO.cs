using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class RecordTypeCreateDTO
    {
        public string RecordName { get; set; } = null!;

        public bool? Status { get; set; }
    }
}
