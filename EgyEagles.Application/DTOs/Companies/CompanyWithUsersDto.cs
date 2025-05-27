using EgyEagles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Companies
{
    // DTOs/CompanyWithUsersDto.cs
    public class CompanyWithUsersDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public List<ApplicationUser> Users { get; set; } = new();
        public List<Vehicle> Vehicles { get; set; } = new();   

    }

}
