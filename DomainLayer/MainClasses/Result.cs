using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.BaseClasses
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private Result(bool isSuccess, string message, T data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success(T data, string message = null)
            => new Result<T>(true, message ?? "Operation successful.", data);

        public static Result<T> Success(string message = null)
            => new Result<T>(true, message ?? "Operation successful.");

        public static Result<T> Failure(string message)
            => new Result<T>(false, message);
    }


}
