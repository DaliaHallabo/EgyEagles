using EgyEagles.Application.Interfaces.Vehicles;
using EgyEagles.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleRepository(IMongoDatabase database)
        {
            _vehicles = database.GetCollection<Vehicle>("Vehicles");
        }

        public async Task<List<Vehicle>> GetAllAsync() =>
            await _vehicles.Find(_ => true).ToListAsync();

        public async Task<List<Vehicle>> GetByCompanyIdAsync(string companyId) =>
            await _vehicles.Find(v => v.CompanyId == companyId).ToListAsync();

        public async Task<Vehicle?> GetByIdAsync(string id) =>
            await _vehicles.Find(v => v.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Vehicle vehicle) =>
            await _vehicles.InsertOneAsync(vehicle);

        public async Task UpdateAsync(Vehicle vehicle) =>
            await _vehicles.ReplaceOneAsync(v => v.Id == vehicle.Id, vehicle);

        public async Task DeleteAsync(string id) =>
            await _vehicles.DeleteOneAsync(v => v.Id == id);
    }


}
