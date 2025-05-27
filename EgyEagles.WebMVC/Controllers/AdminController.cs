using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Users;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public AdminController(IUserService userService, ICompanyService companyService)
        {
            _userService = userService;
            _companyService = companyService;
        }

        public async Task<IActionResult> CreateCompanyAdmin()
        {
            ViewBag.Companies = new SelectList(await _companyService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyAdmin(CreateCompanyAdminDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Companies = new SelectList(await _companyService.GetAllAsync(), "Id", "Name");
                return View(dto);
            }

            var result = await _userService.CreateCompanyAdminAsync(dto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                ViewBag.Companies = new SelectList(await _companyService.GetAllAsync(), "Id", "Name");
                return View(dto);
            }

            return RedirectToAction("CompanyAdmins");
        }

        public async Task<IActionResult> CompanyAdmins()
        {
            var admins = await _userService.GetCompanyAdminsAsync();
            return View(admins);
        }

        public async Task<IActionResult> DeleteCompanyAdmin(Guid id)
        {
            var result = await _userService.DeleteCompanyAdminAsync(id);
            // Optionally handle errors here

            return RedirectToAction("CompanyAdmins");
        }
        [HttpGet]
        public async Task<IActionResult> Details(string userId)
        {
            var userDetails = await _userService.GetUserDetailsAsync(userId);
            if (userDetails == null)
                return NotFound();

            return View(userDetails);
        }

    }

}
