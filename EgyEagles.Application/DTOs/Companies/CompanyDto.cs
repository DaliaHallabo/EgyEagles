using EgyEagles.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Companies
{
    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> UserIds { get; set; } = new();

        public List<UserDto> Users { get; set; } = new();

    }
}
