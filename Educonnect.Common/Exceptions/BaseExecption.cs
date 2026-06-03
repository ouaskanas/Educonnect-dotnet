using Educonnect.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Common.Exceptions
{
    public class BaseExecption : Exception
    {
        public MessageCodes ErrorCode { get; }
        public BaseExecption() : base() { }
        public BaseExecption(string message) : base(message) { }
        public BaseExecption(string message, MessageCodes messageCode) : base(message) { ErrorCode = messageCode; }
    }
}
