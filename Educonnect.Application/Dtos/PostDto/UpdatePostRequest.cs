using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.PostDto
{
    public class UpdatePostRequest
    {
        public string PostTitle { get; set; } = string.Empty;
        public string PostBody { get; set; } = string.Empty;
    }
}
