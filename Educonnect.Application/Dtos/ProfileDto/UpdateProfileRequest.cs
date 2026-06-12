using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.ProfileDto
{
    public class UpdateProfileRequest
    {
        public string username { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phonenumber { get; set; } = string.Empty;
    }
}
