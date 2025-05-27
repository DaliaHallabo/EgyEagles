using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Users
{
    public class UpdateUserDto : CreateUserDto
    {
        public string Id { get; set; }
    }
}
