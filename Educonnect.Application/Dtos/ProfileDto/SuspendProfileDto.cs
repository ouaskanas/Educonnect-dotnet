using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.ProfileDto
{
    public class SuspendProfileDto
    {
        public string ProfileName { get; set; }
        public DateTime? SuspendedAt { get; set; }
        public DateTime? SuspendedUntil { get; set; }
        public Guid? SuspendedBy { get; set; }
    }
}
