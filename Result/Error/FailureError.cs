using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Result.Error
{
    public class FailureError
    {
        public static FailureError Unknown(string message = "UnKnown Error", int statusCode = 0) => new FailureError(FailureErrorType.Unknown, message, statusCode);
        public static FailureError Service(string message) => new FailureError(FailureErrorType.Service, message);
        public static FailureError Validation(string message) => new FailureError(FailureErrorType.Validation, message);
        public static FailureError Database(string message) => new FailureError(FailureErrorType.Database, message);
        public static FailureError Access(string message) => new FailureError(FailureErrorType.Access, message);

        public FailureError(FailureErrorType type, string message, int statusCode = 0)
        {
            Type = type;
            Message = message;
            StatusCode = statusCode;
        }

        public FailureErrorType Type { get; }

        public string Message { get; }

        public int StatusCode { get; }
    }
}
