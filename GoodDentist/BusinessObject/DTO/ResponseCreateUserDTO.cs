using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ResponseCreateUserDTO
    {
        public object? result;

        public bool isSuccess;

        [Required]
        public List<string> message = new List<string>();
    }
}
