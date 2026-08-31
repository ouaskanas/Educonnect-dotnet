using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.GroupDto
{
    public class GetGroupDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public Guid AdminId { get; set; } = Guid.Empty;
        public int PostCount { get; set; } 
        public int MembreCount { get; set; }
        public bool IsMembre { get; set; }
    }
}
