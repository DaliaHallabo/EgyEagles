using EgyEagles.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Companies
{
    public class CompanyDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<UserDto> AdminUsers { get; set; } = new();
    }
}
