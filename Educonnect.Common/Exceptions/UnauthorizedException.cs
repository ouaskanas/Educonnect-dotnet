using Educonnect.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Common.Exceptions
{
    public class UnauthorizedException : BaseExecption
    {
        public UnauthorizedException(string message) : base(message, MessageCodes.Unauthorized) { }
    }
}
