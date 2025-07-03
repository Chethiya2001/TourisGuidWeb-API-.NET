using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models
{
    public class Booking : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("userId")]
        public  string UserId { get; set; }
        [BsonElement("DestinationId")]
        public  string? DestinationId { get; set; }
        [BsonElement("total")]
        public double Total { get; set; } = 0.0;
        [BsonElement("description")]
        public string? Description { get; set; }
        [BsonElement("booking_date")]
        public DateTime? BookedDate { get; set; }
        [BsonElement("start_date")]
        public DateTime? StartDate { get; set; }
        [BsonElement("end_date")]
        public DateTime? EndDate { get; set; }

        [BsonElement("created_At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("updated_At")]
        public DateTime? UpdatedAt { get; set; }
        [BsonElement("status")]
        public BookingStatus? Status { get; set; } = BookingStatus.Confirmed;

        [BsonElement("payment_references")]
        public string? PaymentReference { get; set; }

        public string GetId() => Id;
    };

    public enum BookingStatus
    {
        Confirmed, Cancelled
    }


}