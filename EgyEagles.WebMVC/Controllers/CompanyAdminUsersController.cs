using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "CompanyAdmin")]
    public class CompanyAdminUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPermissionService _permissionService;
        private readonly ICompanyService _companyService;
        private readonly IMongoCollection<ApplicationUser> _userCollection;

        public CompanyAdminUsersController(
            UserManager<ApplicationUser> userManager,
            IPermissionService permissionService,
            ICompanyService companyService,
            IMongoDatabase database)
        {
            _userManager = userManager;
            _permissionService = permissionService;
            _companyService = companyService;
            _userCollection = database.GetCollection<ApplicationUser>("Users");

        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Filter: Same company, not the current user
            var filter = Builders<ApplicationUser>.Filter.And(
                Builders<ApplicationUser>.Filter.Eq(u => u.CompanyId, currentUser.CompanyId),
                Builders<ApplicationUser>.Filter.Ne(u => u.Id, currentUser.Id)
            );

            var allUsers = await _userCollection.Find(filter).ToListAsync();

            var company = await _companyService.GetByIdAsync(currentUser.CompanyId!);
            ViewBag.CompanyName = company?.Name ?? "Your Company";

            ViewBag.CanAddUser = await _permissionService.HasPermissionAsync(currentUser.Id.ToString(), "AddUser");
            ViewBag.CanRemoveUser = await _permissionService.HasPermissionAsync(currentUser.Id.ToString(), "RemoveUser");

            return View(allUsers);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(user.Id.ToString(), "AddUser"))
                return Forbid();

            return View(new CreateCompanyAdminDto()); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyAdminDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(currentUser.Id.ToString(), "AddUser"))
                return Forbid();

            var newUser = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                CompanyId = currentUser.CompanyId
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, dto.Role);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(dto); // ✅ Return the same DTO back to the view
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await _permissionService.HasPermissionAsync(currentUser.Id.ToString(), "DeleteUser"))
                return Forbid();

            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null || userToDelete.CompanyId != currentUser.CompanyId)
                return NotFound();

            await _userManager.DeleteAsync(userToDelete);
            return RedirectToAction(nameof(Index));
        }
    }
}
