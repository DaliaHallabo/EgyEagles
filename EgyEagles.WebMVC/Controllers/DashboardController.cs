using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Vehicles;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Application.Interfaces.Vehicles;
using EgyEagles.Domain.Models;
using EgyEagles.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVehicleService _vehicleService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IUserService userService,
            IVehicleService vehicleService,
            ICompanyService companyService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _vehicleService = vehicleService;
            _companyService = companyService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Map(string companyId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var isSuperAdmin = await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");

            // Default company ID to current user's company
            string selectedCompanyId = currentUser.CompanyId;

            // Allow SuperAdmin to override company selection
            if (isSuperAdmin && !string.IsNullOrEmpty(companyId))
            {
                selectedCompanyId = companyId;
            }

            var vehicles = await _vehicleService.GetByCompanyIdAsync(selectedCompanyId);

            var vehicleDtos = vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                PlateNumber = v.PlateNumber,
                Model = v.Model,
                Type = v.Type,
                Status = v.Status,
                Latitude = v.Latitude != 0 ? v.Latitude : GetRandomLatitude(),
                Longitude = v.Longitude != 0 ? v.Longitude : GetRandomLongitude(),
            }).ToList();

            var viewModel = new MapViewModel
            {
                Vehicles = vehicleDtos,
                IsSuperAdmin = isSuperAdmin,
                SelectedCompanyId = selectedCompanyId,
                Companies = isSuperAdmin ? (await _companyService.GetAllAsync()).ToList() : null
            };

            return View(viewModel);

        }
        private double GetRandomLatitude()
        {
            // Egypt's latitude range (approx)
            var random = new Random();
            return 22.0 + (random.NextDouble() * (31.5 - 22.0));
        }

        private double GetRandomLongitude()
        {
            // Egypt's longitude range (approx)
            var random = new Random();
            return 25.0 + (random.NextDouble() * (35.0 - 25.0));
        }

    }
}
