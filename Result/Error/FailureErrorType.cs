using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Result.Error
{
    public enum FailureErrorType
    {
        Unknown,
        Validation,
        Database,
        Access,
        Service,
        Log
    }
}
