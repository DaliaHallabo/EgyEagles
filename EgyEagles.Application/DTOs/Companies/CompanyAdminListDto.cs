using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Companies
{
    public class CompanyAdminListDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Role {  get; set; }
        public List<string> Permissions { get; set; } = new();

    }

}
