using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserRepository(IMongoDbContext context)
        {
            _users = context.GetCollection<ApplicationUser>("Users");
        }
        public async Task<IReadOnlyCollection<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _users.Find(filter).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var guidId = Guid.Parse(userId); // or Guid.TryParse to be safe
            return await _users.Find(u => u.Id == guidId).FirstOrDefaultAsync();
        }


        public async Task<List<ApplicationUser>> GetAllAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<List<ApplicationUser>> GetByCompanyIdAsync(string companyId)
        {
            return await _users.Find(u => u.CompanyId == companyId).ToListAsync();
        }


        public async Task<List<ApplicationUser>> GetByRoleAsync(string role) =>
            await _users.Find(u => u.Role == role).ToListAsync();

        public async Task<List<ApplicationUser>> GetAdminsByCompanyIdAsync(string companyId)
        {
        

            return await _users.Find(u => u.CompanyId == companyId && u.Role == "CompanyAdmin").ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

    
        public async Task AddAsync(ApplicationUser user) =>
            await _users.InsertOneAsync(user);

        public async Task UpdateAsync(ApplicationUser user) =>
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task DeleteAsync(string id) =>
            await _users.DeleteOneAsync(u => u.Id.ToString() == id);
    }
}
