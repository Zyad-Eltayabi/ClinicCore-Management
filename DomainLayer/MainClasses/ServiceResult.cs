using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.BaseClasses
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private ServiceResult(bool isSuccess, string message, T data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ServiceResult<T> Success(T data, string message = null)
            => new ServiceResult<T>(true, message ?? "Operation successful.", data);

        public static ServiceResult<T> Success(string message = null)
            => new ServiceResult<T>(true, message ?? "Operation successful.");

        public static ServiceResult<T> Failure(string message)
            => new ServiceResult<T>(false, message);
    }


}
