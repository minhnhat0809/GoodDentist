using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ValidationResponseDTO
    {
        public ValidationResponseDTO(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
