using EgyEagles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Vehicles
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<List<Vehicle>> GetByCompanyIdAsync(string companyId);
        Task<Vehicle?> GetByIdAsync(string id);
        Task CreateAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(string id);
    }

}
