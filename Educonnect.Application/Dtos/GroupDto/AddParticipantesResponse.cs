using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.GroupDto
{
    public class AddParticipantesResponse
    {
        public Guid Id { get; set; }
        public List<Dictionary<string, Guid>> Users { get; set; } = new List<Dictionary<string, Guid>>();
        public int Count { get; set; } = 0;
    }
}
