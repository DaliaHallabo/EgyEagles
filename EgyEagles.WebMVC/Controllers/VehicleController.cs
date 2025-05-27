using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Vehicles;
using EgyEagles.Domain.Models;
using EgyEagles.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehicleController(IVehicleService vehicleService,ICompanyService companyService,UserManager<ApplicationUser> userManager)
        {
            _vehicleService = vehicleService;
            _companyService = companyService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return View(vehicles);
        }

        public async Task<IActionResult> Create()
        {
            var companies = await _companyService.GetAllAsync(); // Or your method to get companies
            ViewBag.Companies = new SelectList(companies, "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            await _vehicleService.CreateAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            var companies = await _companyService.GetAllAsync(); // Or your method to get companies

            ViewBag.Companies = new SelectList(companies, "Id", "Name");

            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            await _vehicleService.UpdateAsync(vehicle);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _vehicleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
