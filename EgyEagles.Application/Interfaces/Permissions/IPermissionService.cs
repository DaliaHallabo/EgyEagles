using EgyEagles.Application.DTOs.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Permissions
{
    public interface IPermissionService
    {
       // Task AssignPermissionsAsync(PermissionDto dto);
        Task<List<string>> GetPermissionsAsync(string userId);//used
       // Task RevokePermissionsAsync(PermissionDto dto);
        Task AssignPermissionsToUserAsync(string userId, List<string> selectedPermissions);//used
        Task<bool> HasPermissionAsync(string userId, string permission);//used

    }

}
