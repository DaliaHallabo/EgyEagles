using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Application.DTOs.Vehicles
{
    public class VehicleDto
    {
        public string Id { get; set; }
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
