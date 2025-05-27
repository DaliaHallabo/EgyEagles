using EgyEagles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<List<ApplicationUser>> GetByCompanyIdAsync(string companyId);
        Task<List<ApplicationUser>> GetByRoleAsync(string role);
        Task<List<ApplicationUser>> GetAdminsByCompanyIdAsync(string companyId);
        Task<IReadOnlyCollection<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> filter);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task AddAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
    }
}
