using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ResponseListDTO
    {
        public object? Result {get; set;}
        public bool IsSuccess { get; set; }
        public List<string> Message { get; set; } = new List<string>();
    }
}
