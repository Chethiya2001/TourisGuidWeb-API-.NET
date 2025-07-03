using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static TourisGuidApi.Dtos;

namespace TourisGuidApi.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingController(ILogger<Booking> logger, IRepository<Booking, string> repository) : Controller
    {
        private readonly ILogger<Booking> _logger = logger;
        private readonly IRepository<Booking, string> _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] string? id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var booking = await _repository.GetAsync(id);
                    if (booking != null)

                        return Ok(ApiResponse<object>.SuccessResponse(booking, "Get Booking Successfully."));
                }
                var allBookings = await _repository.GetAllAsync();
                return Ok(ApiResponse<object>.SuccessResponse(allBookings, "Get All Bookings Successfully."));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }

        }
        [HttpPost]
        public async Task<ActionResult> CreateBooking(BookingDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Booking data is required", 400));
                var newBooking = new Booking
                {
                    UserId = dto.UserId,
                    DestinationId = dto.DestinationId,
                    Total = dto.Total,
                    Description = dto.Description,
                    BookedDate = dto.BookedDate,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PaymentReference = dto.PaymentReference
                };
                await _repository.CreateAsync(newBooking);
                return CreatedAtAction(nameof(GetAllBookings),
           ApiResponse<Booking>.SuccessResponse(newBooking, "Booking created successfully.", 201));

            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }
        [HttpPatch]
        public async Task<ActionResult> UpdateBooking([FromQuery] string id, BookingDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(ApiResponse<string>.ErrorResponse("Booking ID is required", 400));

                var existingBooking = await _repository.GetAsync(id);
                if (existingBooking == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Booking not found", 404));
                if (dto.Total != 0) // not ideal because 0 could be a valid update value
                {
                    existingBooking.Total = dto.Total;
                }
                // Apply updates if present
                existingBooking.Total = dto.Total;
                existingBooking.Description = dto.Description ?? existingBooking.Description;
                existingBooking.BookedDate = dto.BookedDate ?? existingBooking.BookedDate;
                existingBooking.StartDate = dto.StartDate ?? existingBooking.StartDate;
                existingBooking.EndDate = dto.EndDate ?? existingBooking.EndDate;
                existingBooking.PaymentReference = dto.PaymentReference ?? existingBooking.PaymentReference;
                existingBooking.Status = dto.Status ?? existingBooking.Status;
                existingBooking.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(existingBooking);

                return Ok(ApiResponse<Booking>.SuccessResponse(existingBooking, "Booking updated successfully.", 202));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }
        [HttpDelete]
        public ActionResult DeleteBooking([FromQuery] string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var booking = _repository.RemoveAsync(id);
                    return Ok(ApiResponse<object>.SuccessResponse(null, "Booking Delete Successfully.", 202));

                }
                return NotFound(ApiResponse<string>.ErrorResponse("Booking Not Found.", 404));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }


    }
}