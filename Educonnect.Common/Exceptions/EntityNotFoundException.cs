using Educonnect.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Common.Exceptions
{
    public class EntityNotFoundException : BaseExecption
    {
        public EntityNotFoundException(string message) :
            base(message, MessageCodes.EntityNotFound) { }

    }
}
