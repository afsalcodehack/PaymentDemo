using Result.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Result
{
    public abstract class Result<T>
    {
        public static Result<T> Success(T value)
        {
            return new Success<T>(value);
        }

        public static Result<T> Failure(FailureError error)
        {
            return new Failure<T>(error);
        }

        public static Result<T> Failure()
        {
            return Failure(FailureError.Unknown());
        }

        public abstract bool IsFailure { get; }
        public abstract bool IsSuccess { get; }

    }
}
