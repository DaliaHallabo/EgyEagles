using EgyEagles.Application.DTOs.Companies;
using EgyEagles.Application.DTOs.Users;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Users;
using EgyEagles.Domain.Models;
using EgyEagles.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMongoCollection<ApplicationUser> _userCollection;
        private readonly IMongoCollection<Vehicle> _vehicleCollection;



        public CompanyService(IMongoDatabase database,ICompanyRepository repository,IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userCollection = database.GetCollection<ApplicationUser>("Users");
            _vehicleCollection = database.GetCollection<Vehicle>("Vehicles");
            _repository = repository;
            _userRepo = userRepository;
            _userManager= userManager;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var companies = await _repository.GetAllAsync();
            return companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Industry = c.Industry,
                Description = c.Description,
                CreatedDate = c.CreatedDate   // map CreatedDate
            });
        }


        public async Task<CompanyDto?> GetByIdAsync(string id)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null) return null;

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Industry = company.Industry,
                Description = company.Description,
            };
        }

        public async Task AddAsync(CreateCompanyDto dto)
        {
            var company = new Company
            {
                Name = dto.Name,
                Industry = dto.Industry,
                Description = dto.Description
            };
            await _repository.AddAsync(company);
        }

        public async Task UpdateAsync(string id, CreateCompanyDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Name = dto.Name;
            existing.Industry = dto.Industry;
            existing.Description = dto.Description;
            await _repository.UpdateAsync(existing);
        }
        public async Task UpdateCompanyAsync(CompanyDto companyDto)
        {
            // Map DTO to domain model
            var company = new Company
            {
                Id = companyDto.Id,
                Name = companyDto.Name,
                Industry = companyDto.Industry,
                Description = companyDto.Description,
                CreatedDate = companyDto.CreatedDate,
                UserIds = companyDto.UserIds // optional, if it exists in DTO
            };

            // Now update the company using repository
            await _repository.UpdateAsync(company);
        }


        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
        //public async Task<CompanyDetailsDto> GetCompanyDetailsAsync(string companyId)
        //{
        //    var company = await _repository.GetByIdAsync(companyId);
        //    if (company == null) return null;

        //    var users = await _userRepo.GetAdminsByCompanyIdAsync(companyId);

        //    var admins = new List<UserDto>();
        //    foreach (var user in users)
        //    {
        //        if (await _userManager.IsInRoleAsync(user, "CompanyAdmin"))
        //        {
        //            admins.Add(new UserDto
        //            {
        //                Id = user.Id,
        //                Email = user.Email,
        //                Role = "CompanyAdmin"
        //            });
        //        }
        //    }

        //    return new CompanyDetailsDto
        //    {
        //        Id = company.Id,
        //        Name = company.Name,
        //        AdminUsers = admins
        //    };
        //}
        // Pseudocode for your company service
        public async Task<CompanyWithUsersDto> GetCompanyWithUsersAsync(string companyId)
        {
            var company = await _repository.GetByIdAsync(companyId);
            if (company == null) return null;

            var users = await _userCollection.Find(c => c.CompanyId == companyId).ToListAsync();
            var vehicles = await _vehicleCollection
                                 .Find(v => v.CompanyId == companyId)
                                 .ToListAsync();
            return new CompanyWithUsersDto
            {
                Id = company.Id,
                Name = company.Name,
                Industry = company.Industry,
                Description = company.Description,
                Users = users ,
                Vehicles = vehicles,
                CreatedDate= company.CreatedDate,

            };
        }

       
    }
    

}
