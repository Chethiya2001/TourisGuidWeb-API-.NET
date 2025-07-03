
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models
{
    public class Destination : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        [BsonElement("title")]
        public required string Title { get; set; }
        [BsonElement("sub_tittle")]
        public string? SubTitle { get; set; }
        [BsonElement("Description")]
        public string? Description { get; set; }
        [BsonElement("other")]
        public string? Other { get; set; }
        [BsonElement("email")]
        public string? Email { get; set; }
        [BsonElement("contact_number")]
        public int? ContactNumber { get; set; }
        [BsonElement("address")]
        public required string Address { get; set; }
        [BsonElement("location")]
        public required string Location { get; set; }
        [BsonElement("latitude")]
        public required float Latitude { get; set; }
        [BsonElement("longitude")]
        public required float Longitude { get; set; }
        [BsonElement("images")]
        public List<string>? Images { get; set; } = [];
        [BsonElement("created_At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("updated_At")]
        public DateTime UpdatedAt { get; set; }
        public string GetId() => Id;

    }
}