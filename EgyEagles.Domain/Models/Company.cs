using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Domain.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // MongoDB ObjectId as string

        public string Name { get; set; }
        public string Industry { get; set; }

        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public List<string> UserIds { get; set; } = new();
        [BsonIgnore]  // <- Add this to ignore Users property in MongoDB
        public virtual ICollection<ApplicationUser>? Users { get; set; }



    }
}
