using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Vehicles;

namespace EgyEagles.WebMVC.Models
{
    public class MapViewModel
    {
        public bool IsSuperAdmin { get; set; }
        public string? SelectedCompanyId { get; set; }
        public List<CompanyDto> Companies { get; set; }
        public List<VehicleDto> Vehicles { get; set; }
    }

}
