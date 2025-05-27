using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Domain.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public List<string> Permissions { get; set; } = new();

        public string? FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CompanyId { get; set; }  // also string but represented as ObjectId in MongoDB
        public virtual Company? Company { get; set; }

    }
}
