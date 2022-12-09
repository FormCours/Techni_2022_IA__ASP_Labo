using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Domain.Enums
{
    public enum RegistrationResult
    {
        Success,
        ActivityNotFound,
        AlreadyExists,
        NotExists,
        TooManyGuest,
        Error
    }
}
