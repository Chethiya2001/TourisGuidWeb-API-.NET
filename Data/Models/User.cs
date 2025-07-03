using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models
{
    public class User : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("first_name")]
        public string? FirstName { get; set; }
        [BsonElement("last_name")]
        public string? LastName { get; set; }
        [BsonElement("email")]
        public string? Email { get; set; }
        [BsonElement("mobile")]
        public string? Mobile { get; set; }

        [BsonElement("profile_image")]
        public string? ProfileImage { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }
        [BsonElement("city")]
        public string? City { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("country")]
        public string? Country { get; set; }
        [BsonElement("zipcode")]
        public string? ZipCode { get; set; }
        [BsonElement("role")]
        public string? Role { get; set; } = "user";

        [BsonElement("created_At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("updated_At")]
        public DateTime UpdatedAt { get; set; }
        public string GetId() => Id;

    }
}