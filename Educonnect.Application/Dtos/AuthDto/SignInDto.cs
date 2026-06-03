using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.AuthDto
{
    public class SignInDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
