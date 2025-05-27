using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Users;
using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<List<CompanyAdminListDto>> GetCompanyAdminsAsync();//used
      Task<IdentityResult> CreateCompanyAdminAsync(CreateCompanyAdminDto dto);
     //   Task<EditCompanyAdminDto> GetCompanyAdminForEditAsync(Guid id);
    //   Task<IdentityResult> UpdateCompanyAdminAsync(Guid id, EditCompanyAdminDto dto);
        Task<IdentityResult> DeleteCompanyAdminAsync(Guid id);//used
     //   Task<List<ApplicationUser>> GetAdminsByCompanyIdAsync(string companyId);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();//used
        Task<ApplicationUser> GetUserByIdAsync(string userId);//used
        Task<UserDto> GetUserDetailsAsync(string userId);//used




    }
}
