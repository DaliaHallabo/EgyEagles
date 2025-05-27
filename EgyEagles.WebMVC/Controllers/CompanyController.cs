using EgyEagles.Application.DTOs;
using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.Interfaces;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IPermissionService _permissionService;

        public CompanyController(ICompanyService companyService,IPermissionService permission)
        {
            _companyService = companyService;
            _permissionService = permission;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAllAsync();
            return View(companies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _companyService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();

            var dto = new CreateCompanyDto
            {
                Name = company.Name,
                Industry = company.Industry,
                Description = company.Description
            };
            ViewBag.CompanyId = id;
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CreateCompanyDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CompanyId = id;
                return View(dto);
            }

            await _companyService.UpdateAsync(id, dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _companyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var companyWithUsers = await _companyService.GetCompanyWithUsersAsync(id);

            return View(companyWithUsers);
        }
 

        [HttpPost]
        public async Task<IActionResult> UpdatePermissionsForCompanyAdmin(string userId, List<string> selectedPermissions)
        {
            await _permissionService.AssignPermissionsToUserAsync(userId, selectedPermissions);
            return RedirectToAction("CompanyAdmins"); 
        }

    }
}
