using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Users
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string CompanyId { get; set; }
    }
}
