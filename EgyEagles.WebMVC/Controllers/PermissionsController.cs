using EgyEagles.Application.DTOs.Permissions;
using EgyEagles.Application.DTOs.Users;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Domain.Models;
using EgyEagles.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EgyEagles.WebMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PermissionsController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        public PermissionsController(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Permissions()
        {
            var users = await _userService.GetAllUsersAsync();
            var model = new PermissionsViewModel
            {
                Users = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var currentPermissions = await _permissionService.GetPermissionsAsync(userId);

            ViewBag.UserId = userId;
            ViewBag.UserName = $"{user.FirstName} {user.LastName}";
            ViewBag.CurrentPermissions = currentPermissions;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePermissionsForCompanyAdmin(string userId, List<string> selectedPermissions)
        {
            await _permissionService.AssignPermissionsToUserAsync(userId, selectedPermissions);
            return RedirectToAction("CompanyAdmins", "Admin");
        }
    }
}