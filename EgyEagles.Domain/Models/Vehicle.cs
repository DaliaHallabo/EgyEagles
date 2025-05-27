using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Domain.Models
{
    public class Vehicle
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [Required]
        [RegularExpression("Available|InUse", ErrorMessage = "Status must be either Available or InUse.")]
        public string Status { get; set; }

        public string? CompanyId { get; set; }
        public double Latitude { get; set; } = GetRandomCoordinate(22.0, 31.5); // Egypt's latitude range
        public double Longitude { get; set; } = GetRandomCoordinate(25.0, 35.0); // Egypt's longitude range
        private static double GetRandomCoordinate(double min, double max)
        {
            var random = new Random();
            return min + (random.NextDouble() * (max - min));
        }
    }

}
