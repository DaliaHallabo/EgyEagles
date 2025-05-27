using EgyEagles.Application.Interfaces.Permissions;
using EgyEagles.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IMongoCollection<Permission> _permissionCollection;

        public PermissionRepository(IMongoDatabase database)
        {
            _permissionCollection = database.GetCollection<Permission>("Permissions");
        }

        public async Task SetUserPermissionsAsync(string userId, List<string> permissions)
        {
            var filter = Builders<Permission>.Filter.Eq(p => p.UserId, userId);
            var update = Builders<Permission>.Update.Set(p => p.PermissionList, permissions);

            var result = await _permissionCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }

}
