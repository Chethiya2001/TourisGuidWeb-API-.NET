using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourisGuidApi.Helpers;
using static TourisGuidApi.Dtos;

namespace TourisGuidApi.Controllers
{
    [ApiController]
    [Route("api/vi/user")]
    public class UserController(IConfiguration config, IRepository<User, string> repository) : ControllerBase
    {
        private readonly IRepository<User, string> _repository = repository;
        private readonly IConfiguration _config = config;
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var User = await _repository.GetAsync(id);
                    if (User != null)

                        return Ok(ApiResponse<object>.SuccessResponse(User, "Get User Successfully."));
                }
                var allUsers = await _repository.GetAllAsync();
                return Ok(ApiResponse<object>.SuccessResponse(allUsers, "Get All Users Successfully."));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }

        }
        [HttpPost("edit")]
        [Authorize]
        public async Task<ActionResult> EditUser(UserDto dto)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid token", 401));

                var existingUser = await _repository.GetAsync(userId);

                if (existingUser == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found", 404));


                existingUser.FirstName = dto.FirstName;
                existingUser.LastName = dto.LastName;
                existingUser.Address = dto.Address;
                existingUser.City = dto.City;
                existingUser.Country = dto.Country;
                existingUser.Mobile = dto.Mobile;
                existingUser.ProfileImage = dto.ProfileImage;
                existingUser.UpdatedAt = DateTime.UtcNow;


                await _repository.UpdateAsync(existingUser);
                return CreatedAtAction(nameof(GetAllUsers),
           ApiResponse<User>.SuccessResponse(existingUser, "User Data Edit successfully.", 202));

            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid token. User ID not found.", 401));
                }

                var user = await _repository.GetAsync(userId);

                if (user == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found.", 404));
                }

                return Ok(ApiResponse<object>.SuccessResponse(user, "User retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }

        [HttpPost("remove")]
        public ActionResult DeleteUser([FromQuery] string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var User = _repository.RemoveAsync(id);
                    return Ok(ApiResponse<object>.SuccessResponse(null, "User Delete Successfully.", 202));

                }
                return NotFound(ApiResponse<string>.ErrorResponse("User Not Found.", 404));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }
        [HttpPost("auth/register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Password))
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Email and Password are required", 400));
                }

                // Await this async call to get the actual user object
                var existingUser = await _repository.GetAsync(u => u.Email == userDto.Email);

                if (existingUser != null)
                {
                    return Conflict(ApiResponse<string>.ErrorResponse("User with this email already exists", 409));
                }

                var hashPassword = PasswordHasherHelper.HashPassword(userDto.Password!);

                var registerNewUser = new User
                {
                    Email = userDto.Email,
                    Password = hashPassword
                };

                await _repository.CreateAsync(registerNewUser);

                return Ok(ApiResponse<object>.SuccessResponse(registerNewUser, "Register New Users Successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }
        }

        [HttpPost("auth/login")]
        public async Task<ActionResult> Login([FromBody] UserDto user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                    return BadRequest(ApiResponse<string>.ErrorResponse("Email and Password are required", 400));
                var existingUser = await _repository.GetAsync(u => u.Email == user.Email);

                if (existingUser == null)
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid credentials", 401));

                var isPasswordValid = PasswordHasherHelper.VerifyPassword(user.Password, existingUser.Password!);
                if (!isPasswordValid)
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid credentials", 401));

                var token = JWTHelper.GenerateToken(existingUser.Id, existingUser.Role ?? "user", _config);

                return Ok(ApiResponse<object>.SuccessResponse(new
                {
                    token = token,
                    user = existingUser,

                }, "Login Successfully."));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse($"An error occurred: {ex.Message}", 500));
            }

        }
    }

}