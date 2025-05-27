using EgyEagles.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var mongoDatabase = serviceProvider.GetRequiredService<IMongoDatabase>();

            var companyCollection = mongoDatabase.GetCollection<Company>("Companies");
            var vehicleCollection = mongoDatabase.GetCollection<Vehicle>("Vehicles");

            // Seed Roles
            var roles = new[] { "SuperAdmin", "CompanyAdmin", "RegularUser" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }

            // Seed Company
            var existingCompany = await companyCollection.Find(_ => true).FirstOrDefaultAsync();
            Company company;
            if (existingCompany == null)
            {
                company = new Company
                {
                    Name = "Seeded Company",
                };
                await companyCollection.InsertOneAsync(company);
            }
            else
            {
                company = existingCompany;
            }

            // Seed Super Admin
            string superAdminEmail = "superadmin@egyeagles.com";
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdminUser == null)
            {
                superAdminUser = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }
            }

            // Seed Company Admin
            string companyAdminEmail = "companyadmin@egyeagles.com";
            var companyAdminUser = await userManager.FindByEmailAsync(companyAdminEmail);
            if (companyAdminUser == null)
            {
                companyAdminUser = new ApplicationUser
                {
                    UserName = companyAdminEmail,
                    Email = companyAdminEmail,
                    EmailConfirmed = true,
                    FirstName = "Company",
                    LastName = "Admin",
                    CompanyId = company.Id
                };

                var result = await userManager.CreateAsync(companyAdminUser, "CompanyAdmin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(companyAdminUser, "CompanyAdmin");
                }
            }

            // Seed Regular User
            string regularUserEmail = "user@egyeagles.com";
            var regularUser = await userManager.FindByEmailAsync(regularUserEmail);
            if (regularUser == null)
            {
                regularUser = new ApplicationUser
                {
                    UserName = regularUserEmail,
                    Email = regularUserEmail,
                    EmailConfirmed = true,
                    FirstName = "Regular",
                    LastName = "User",
                    CompanyId = company.Id
                };

                var result = await userManager.CreateAsync(regularUser, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "RegularUser");
                }
            }

            // Seed Vehicles
            var existingVehicles = await vehicleCollection.Find(_ => true).ToListAsync();
            if (existingVehicles.Count == 0)
            {
                var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    PlateNumber = "ABC123",
                    Model = "Toyota Corolla",
                    Type = "Sedan",
                    Status = "Available",
                    CompanyId = company.Id
                },
                new Vehicle
                {
                    PlateNumber = "XYZ789",
                    Model = "Ford Ranger",
                    Type = "Truck",
                    Status = "In Use",
                    CompanyId = company.Id
                }
            };

                await vehicleCollection.InsertManyAsync(vehicles);
            }

        }
    }
}
