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
        public ServiceErrorType ErrorType { get; private set; } = ServiceErrorType.None;


        private ServiceResult(ServiceErrorType errorType, bool isSuccess, string message, T data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            ErrorType = errorType;
        }

        public static ServiceResult<T> Success(T data)
            => new ServiceResult<T>(ServiceErrorType.Success, true, string.Empty, data);

        public static ServiceResult<T> Success()
            => new ServiceResult<T>(ServiceErrorType.Success, true, string.Empty);

        public static ServiceResult<T> Failure(string message, ServiceErrorType ErrorType)
            => new ServiceResult<T>(ErrorType, false, message);
    }

    // Error types that can be mapped to HTTP status codes in the API layer
    public enum ServiceErrorType
    {
        None = 0,
        Success = 1,
        ValidationError = 400,   // Maps to 400 Bad Request
        NotFound = 404,          // Maps to 404 Not Found
        Conflict = 409,          // Maps to 409 Conflict
        DatabaseError = 503,   // Maps to 503 Service Unavailable
        ServerError = 500        // Maps to 500 Internal Server Error
    }

}
