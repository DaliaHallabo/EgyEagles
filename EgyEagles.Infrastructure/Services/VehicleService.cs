using EgyEagles.Application.DTOs.Vehicles;
using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Application.Interfaces.Vehicles;
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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMongoCollection<Vehicle> _vehicleCollection;
        private readonly IMongoCollection<Company> _companyCollection;


        public VehicleService(IMongoDatabase database,IVehicleRepository vehicleRepo, ICompanyRepository companyRepo, UserManager<ApplicationUser> userManager)
        {
            _vehicleCollection = database.GetCollection<Vehicle>("Vehicles");
            _companyCollection = database.GetCollection<Company>("Companies");

            _vehicleRepo = vehicleRepo;
            _companyRepo = companyRepo;
            _userManager = userManager;

        }

        public async Task<List<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _vehicleRepo.GetAllAsync();
            var companies = await _companyRepo.GetAllAsync();

            var result = vehicles.Select(v =>
            {
                var company = companies.FirstOrDefault(c => c.Id == v.CompanyId);
                return new VehicleDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    Model = v.Model,
                    Type = v.Type,
                    Status = v.Status,
                    CompanyName = company?.Name ?? "Unknown"
                };
            }).ToList();

            return result;
        }
        public async Task<List<VehicleDto>> GetVehiclesForUserAsync(ApplicationUser user)
        {
            if (user == null)
                return new List<VehicleDto>();

            if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                var vehicles = await _vehicleCollection.Find(_ => true).ToListAsync();

                var result = new List<VehicleDto>();
                foreach (var v in vehicles)
                {
                    var company = await _companyCollection.Find(c => c.Id == v.CompanyId).FirstOrDefaultAsync();
                    result.Add(new VehicleDto
                    {
                        Id = v.Id,
                        PlateNumber = v.PlateNumber,
                        Model = v.Model,
                        Type = v.Type,
                        Status = v.Status,
                        CompanyName = company?.Name ?? "Unknown"
                    });
                }

                return result;
            }

            if (await _userManager.IsInRoleAsync(user, "CompanyAdmin") && !string.IsNullOrEmpty(user.CompanyId))
            {
                var vehicles = await _vehicleCollection.Find(v => v.CompanyId == user.CompanyId).ToListAsync();

                var company = await _companyCollection.Find(c => c.Id == user.CompanyId).FirstOrDefaultAsync();

                var result = vehicles.Select(v => new VehicleDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    Model = v.Model,
                    Type = v.Type,
                    Status = v.Status,
                    CompanyName = company?.Name ?? "Unknown"
                }).ToList();

                return result;
            }

            return new List<VehicleDto>();
        }


        public Task<List<Vehicle>> GetByCompanyIdAsync(string companyId) => _vehicleRepo.GetByCompanyIdAsync(companyId);
        public Task<Vehicle?> GetByIdAsync(string id) => _vehicleRepo.GetByIdAsync(id);
        public async Task CreateAsync(Vehicle vehicle)
        {
            try
            {
                await _vehicleCollection.InsertOneAsync(vehicle);
            }
            catch (Exception ex)
            {
                // Log it or rethrow to see the issue
                throw new Exception("Error inserting vehicle", ex);
            }
        }
        public Task UpdateAsync(Vehicle vehicle) => _vehicleRepo.UpdateAsync(vehicle);
        public Task DeleteAsync(string id) => _vehicleRepo.DeleteAsync(id);
    }


}
