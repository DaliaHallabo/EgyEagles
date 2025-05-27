using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Users;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Domain.Models;
using EgyEagles.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICompanyService _companyService;
        private readonly IPermissionService _permissionService;

        public UserService(IUserRepository userRepo, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ICompanyService companyService,IPermissionService permissionService)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _roleManager = roleManager;
            _companyService = companyService;
            _permissionService = permissionService;
        }

        public async Task<List<CompanyAdminListDto>> GetCompanyAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync(Roles.CompanyAdmin);
            var companies = await _companyService.GetAllAsync();

            return admins.Select(admin => new CompanyAdminListDto
            {
                Id = admin.Id,
                Email = admin.Email,
                FullName = admin.FullName,
                Role=admin.Role,
                CompanyName = companies.FirstOrDefault(c => c.Id == admin.CompanyId?.ToString())?.Name ?? "Unassigned"
            }).ToList();
        }


        public async Task<IdentityResult> CreateCompanyAdminAsync(CreateCompanyAdminDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                CompanyId = dto.CompanyId,
                Role=dto.Role,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return result;

            // After successfully creating the user
            if (!string.IsNullOrEmpty(user.CompanyId))
            {
                var company = await _companyService.GetByIdAsync(user.CompanyId);
                if (company != null)
                {
                    if (company.UserIds == null)
                        company.UserIds = new List<string>();

                    if (!company.UserIds.Contains(user.Id.ToString()))
                    {
                        company.UserIds.Add(user.Id.ToString());

                        // Create a CreateCompanyDto from the company domain model
                        var updateDto = new CreateCompanyDto
                        {
                            Name = company.Name,
                            Industry = company.Industry,
                            Description = company.Description,
                            // Add other properties as needed
                            UserIds = company.UserIds // Assuming your DTO has this property; otherwise, omit or handle accordingly
                        };

                        // Call UpdateAsync with id and dto
                        await _companyService.UpdateAsync(company.Id, updateDto);
                    }
                }
            }

            // Add to "CompanyAdmin" role
            await _userManager.AddToRoleAsync(user, "CompanyAdmin");

            // Set Role manually if needed in your MongoDB model
            user.Role = "CompanyAdmin"; // only if you're manually tracking it
            await _userRepo.UpdateAsync(user); // if you’re persisting role manually

            return IdentityResult.Success;
        }


        public async Task<EditCompanyAdminDto> GetCompanyAdminForEditAsync(Guid id)
        {
            var admin = await _userManager.FindByIdAsync(id.ToString());
            if (admin == null) return null;

            return new EditCompanyAdminDto
            {
                Email = admin.Email,
                FullName = admin.FullName,
                CompanyId = admin.CompanyId,
                Role = (await _userManager.GetRolesAsync(admin)).FirstOrDefault()
            };


        }
        public async Task<IdentityResult> UpdateCompanyAdminAsync(Guid id, EditCompanyAdminDto dto)
        {
            var admin = await _userManager.FindByIdAsync(id.ToString());
            if (admin == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            admin.Email = dto.Email;
            admin.UserName = dto.Email;
            admin.FullName = dto.FullName;
            admin.Role = dto.Role;
            if (string.IsNullOrEmpty(dto.CompanyId))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid Company ID." });

            admin.CompanyId = dto.CompanyId;





            var updateResult = await _userManager.UpdateAsync(admin);
            if (!updateResult.Succeeded) return updateResult;

            var currentRoles = await _userManager.GetRolesAsync(admin);
            await _userManager.RemoveFromRolesAsync(admin, currentRoles);

            var addRoleResult = await _userManager.AddToRoleAsync(admin, dto.Role);
            if (!addRoleResult.Succeeded) return addRoleResult;

            return IdentityResult.Success;
        }


        public async Task<IdentityResult> DeleteCompanyAdminAsync(Guid id)
        {
            var admin = await _userManager.FindByIdAsync(id.ToString());
            if (admin == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            return await _userManager.DeleteAsync(admin);
        }

        public async Task<List<ApplicationUser>> GetAdminsByCompanyIdAsync(string companyId)
        {
            var users = await _userRepo.GetByCompanyIdAsync(companyId);
            var adminUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "CompanyAdmin"))
                {
                    adminUsers.Add(user);
                }
            }

            return adminUsers;
        }


        private UserDto MapToDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
            };
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userRepo.GetUserByIdAsync(userId);
        }

        public async Task<UserDto> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            // Fetch the company manually
            string companyName = "Unassigned";
            if (!string.IsNullOrEmpty(user.CompanyId))
            {
                var company = await _companyService.GetByIdAsync(user.CompanyId);
                companyName = company?.Name ?? "Unassigned";
            }

            var permissions = await _permissionService.GetPermissionsAsync(userId);

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CompanyName = companyName,
                Permissions = permissions
            };
        }
    }
}
