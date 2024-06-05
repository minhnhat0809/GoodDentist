using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ResponseDTO
    {
        public string Message {  get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public ResponseDTO(string message, int statusCode, bool isSucess, object? result)
        {
            Message = message;
            StatusCode = statusCode;
            IsSuccess = isSucess;
            Result = result;
        }

    }
}
