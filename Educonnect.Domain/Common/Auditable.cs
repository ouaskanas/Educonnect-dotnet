using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Common
{
    public abstract class Auditable
    {
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set;}
        public Guid? ModifedBy { get; set; }
        public bool IsModified { get; set; }
        
        public virtual void Delete(Guid userId)
        {
            DeletedAt = DateTime.Now;
            DeletedBy = userId;
            IsDeleted = true;
        }

        public virtual void Modify(Guid userId)
        {
            ModifedBy = userId;
            LastModifiedAt = DateTime.Now;
            IsModified = true;
        }
    }
}
