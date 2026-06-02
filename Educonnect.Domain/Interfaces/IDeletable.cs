using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Interfaces
{
    public interface IDeletable
    {
        DateTime? DeletedAt { get; set; }
        bool IsDeleted { get; set; }
        Guid? DeletedBy { get; set; }
    }
}
