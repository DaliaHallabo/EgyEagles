using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Application.Interfaces.Vehicles;
using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "CompanyAdmin")]
    public class CompanyAdminVehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPermissionService _permissionService;

        public CompanyAdminVehiclesController(
            IVehicleService vehicleService,
            UserManager<ApplicationUser> userManager,
            ICompanyService companyService,
            IPermissionService permissionService)
        {
            _vehicleService = vehicleService;
            _userManager = userManager;
            _companyService = companyService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vehicles = await _vehicleService.GetVehiclesForUserAsync(user);

            var company = await _companyService.GetByIdAsync(user.CompanyId!);
            ViewBag.CompanyName = company?.Name ?? "Your Company";

            // Check permissions and pass to the view
            ViewBag.CanAddVehicle = await _permissionService.HasPermissionAsync(user.Id.ToString(), "AddVehicle");
            ViewBag.CanRemoveVehicle = await _permissionService.HasPermissionAsync(user.Id.ToString(), "RemoveVehicle");

            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(user.Id.ToString(), "AddVehicle"))
                return Forbid();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(user.Id.ToString(), "AddVehicle"))
                return Forbid();

            vehicle.CompanyId = user.CompanyId;
            await _vehicleService.CreateAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(user.Id.ToString(), "RemoveVehicle"))
                return Forbid();

            await _vehicleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }

}
