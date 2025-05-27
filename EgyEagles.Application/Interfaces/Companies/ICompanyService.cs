using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Companies
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllAsync();//used
        Task<CompanyDto?> GetByIdAsync(string id);//used
        Task AddAsync(CreateCompanyDto dto);//used
        Task UpdateAsync(string id, CreateCompanyDto dto);//used
        Task DeleteAsync(string id);//used
        Task<CompanyWithUsersDto> GetCompanyWithUsersAsync(string companyId);//used

        Task UpdateCompanyAsync(CompanyDto companyDto);
//

    }

}
