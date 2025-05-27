using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Permissions
{
    public class PermissionDto
    {
        public string UserId { get; set; }
        public List<string> Permissions { get; set; } = new();
    }

}
