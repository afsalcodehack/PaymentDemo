using Result.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Result
{
    public class Failure<T> : Result<T>
    {
        public Failure(FailureError error)
        {
            Error = error;
        }

        public FailureError Error { get; }

     
        public override bool IsFailure => true;
        public override bool IsSuccess => false;

    }
}
