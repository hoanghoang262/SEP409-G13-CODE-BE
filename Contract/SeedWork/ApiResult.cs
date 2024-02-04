using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.SeedWork
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        
        public ApiResult(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

       
        public ApiResult(string message)
        {
            Success = false;
            Message = message;
        }

        
        public static ApiResult<T> SuccessResult(T data, string message = "") => new ApiResult<T>(data, message);
        public static ApiResult<T> FailureResult(string message) => new ApiResult<T>(message);
    }
}
