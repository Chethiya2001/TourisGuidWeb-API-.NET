
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;

namespace TourisGuidApi
{
    public class Dtos
    {
        public record DestinationCreateDto
        {
            public required string Title { get; set; }
            public string? SubTitle { get; set; }
            public string? Description { get; set; }
            public string? Other { get; set; }
            public string? Email { get; set; }
            public int? ContactNumber { get; set; }
            public required string Address { get; set; }
            public required string Location { get; set; }
            public required float Latitude { get; set; }
            public required float Longitude { get; set; }
            public List<string>? Images { get; set; } = new List<string>();
        }
        public record UserDto
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? ProfileImage { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Country { get; set; }
            public string? ZipCode { get; set; }
            public string? Role { get; set; }

            public string? Password { get; set; }
        }
        public record BookingDto
        {

            public required string UserId { get; set; }
            public string? DestinationId { get; set; }
            public double Total { get; set; }
            public string? Description { get; set; }
            public DateTime? BookedDate { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public BookingStatus? Status { get; set; }
            public string? PaymentReference { get; set; }
        }
    }
}