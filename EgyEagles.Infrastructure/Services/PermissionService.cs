using EgyEagles.Application.DTOs.Permissions;
using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMongoCollection<ApplicationUser> _userCollection;
        private readonly IMongoCollection<Permission> _permissionsCollection;
        private readonly IPermissionRepository _permissionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(IMongoDatabase database,IPermissionRepository permissionRepository, UserManager<ApplicationUser> userManager)
        {
            _userCollection = database.GetCollection<ApplicationUser>("Users");
            _permissionsCollection = database.GetCollection<Permission>("Permissions");
            _userManager = userManager;
            _permissionRepository = permissionRepository;

        }

        public async Task AssignPermissionsAsync(PermissionDto dto)
        {
            var userId = Guid.Parse(dto.UserId);
            var update = Builders<ApplicationUser>.Update
                .AddToSetEach(u => u.Permissions, dto.Permissions);

            await _userCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        public async Task<List<string>> GetPermissionsAsync(string userId)
        {
            var record = await _permissionsCollection
                .Find(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            return record?.PermissionList ?? new List<string>();
        }

        public async Task RevokePermissionsAsync(PermissionDto dto)
        {
            var userId = Guid.Parse(dto.UserId);
            var update = Builders<ApplicationUser>.Update
                .PullAll(u => u.Permissions, dto.Permissions);

            await _userCollection.UpdateOneAsync(u => u.Id == userId, update);
        }
        public async Task AssignPermissionsToUserAsync(string userId, List<string> selectedPermissions)
        {
            await _permissionRepository.SetUserPermissionsAsync(userId, selectedPermissions);
        }
        public async Task<bool> HasPermissionAsync(string userId, string permission)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // SuperAdmins have all permissions
            if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                return true;

            var userPermission = await _permissionsCollection
                .Find(p => p.UserId == userId)
                .FirstOrDefaultAsync();

            return userPermission?.PermissionList.Contains(permission) ?? false;
        }
    }

}
