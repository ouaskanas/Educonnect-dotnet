using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.AuthDto
{
    public class SignUpDto
    {
        public required string Username { get; set; } = string.Empty;
        public required string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string PasswordConfirmation { get; set; } = string.Empty;
    }
}
