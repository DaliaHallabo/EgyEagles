using EgyEagles.Application.DTOs.Vehicles;
using EgyEagles.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.Interfaces.Vehicles
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync();//used
        Task<List<Vehicle>> GetByCompanyIdAsync(string companyId);//used
        Task<Vehicle?> GetByIdAsync(string id);//used
        Task CreateAsync(Vehicle vehicle);//used
        Task UpdateAsync(Vehicle vehicle);//used
        Task DeleteAsync(string id);//used
        Task<List<VehicleDto>> GetVehiclesForUserAsync(ApplicationUser user);//used
    }


}
