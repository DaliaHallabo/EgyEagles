using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Permissions
{
    public interface IPermissionRepository
    {
        Task SetUserPermissionsAsync(string userId, List<string> permissions);

    }
}
