
using Data;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using static TourisGuidApi.Dtos;

namespace TourisGuidApi.Controllers
{
    [ApiController]
    [Route("api/v1/destination")]
    public class DestinationController(ILogger<Destination> logger, IRepository<Destination, string> repository) : Controller
    {
        private readonly ILogger<Destination> _logger = logger;
        private readonly IRepository<Destination, string> _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAllDestinations([FromQuery] string? id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var Destination = await _repository.GetAsync(id);
                    if (Destination != null)

                        return Ok(ApiResponse<object>.SuccessResponse(Destination, "Get Destination Successfully."));
                }
                var allDestinations = await _repository.GetAllAsync();
                return Ok(ApiResponse<object>.SuccessResponse(allDestinations, "Get All Destinations Successfully."));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }

        }
        [HttpPost]
        public async Task<ActionResult> CreateDestination(DestinationCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Destination data is required", 400));
                var newDestination = new Destination
                {
                    Title = dto.Title,
                    SubTitle = dto.SubTitle,
                    Description = dto.Description,
                    Other = dto.Other,
                    Email = dto.Email,
                    ContactNumber = dto.ContactNumber,
                    Address = dto.Address,
                    Location = dto.Location,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Images = dto.Images ?? new List<string>(),
                    UpdatedAt = DateTime.UtcNow
                };
                await _repository.CreateAsync(newDestination);
                return CreatedAtAction(nameof(GetAllDestinations),
           ApiResponse<Destination>.SuccessResponse(newDestination, "Destination created successfully.", 201));

            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }
        [HttpPost("edit")]
        public async Task<ActionResult> UpdateDestination([FromQuery] string id, DestinationCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(ApiResponse<string>.ErrorResponse("Destination ID is required", 400));

                var existingDestination = await _repository.GetAsync(id);
                if (existingDestination == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Destination not found", 404));

                // Apply updates if present
                existingDestination.Title = dto.Title;
                existingDestination.SubTitle = dto.SubTitle;
                existingDestination.Description = dto.Description;
                existingDestination.Other = dto.Other;
                existingDestination.Email = dto.Email;
                existingDestination.ContactNumber = dto.ContactNumber;
                existingDestination.Address = dto.Address;
                existingDestination.Location = dto.Location;
                existingDestination.Latitude = dto.Latitude;
                existingDestination.Longitude = dto.Longitude;
                existingDestination.Images = dto.Images ?? new List<string>();
                existingDestination.UpdatedAt = DateTime.UtcNow;


                await _repository.UpdateAsync(existingDestination);

                return Ok(ApiResponse<Destination>.SuccessResponse(existingDestination, "Destination updated successfully.", 202));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }
        [HttpPost("remove")]
        public ActionResult DeleteDestination([FromQuery] string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var Destination = _repository.RemoveAsync(id);
                    return Ok(ApiResponse<object>.SuccessResponse(null, "Destination Delete Successfully.", 202));

                }
                return NotFound(ApiResponse<string>.ErrorResponse("Destination Not Found.", 404));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }


    }
}