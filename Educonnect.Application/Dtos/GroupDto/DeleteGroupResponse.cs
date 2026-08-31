using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.GroupDto
{
    public class DeleteGroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; }
    }
}
