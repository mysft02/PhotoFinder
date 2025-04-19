using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoFinder.DTO.Booking;
using PhotoFinder.Entity;
using PhotoFinder.Infrastructure;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http; // Add this for HttpContext

namespace PhotoFinder.Infrastructure.Service
{
    public interface IUserService
    {
        Task<IActionResult> HandleGetAllUsers();
        Task<IActionResult> HandleGetUserById(int id);
        Task<IActionResult> HandleCreateUser(User user);
        Task<IActionResult> HandleUpdateUser(User user);
        Task<IActionResult> HandleDeleteUser(int id);
        Task<IActionResult> HandleGetCurrentUser(); // Changed signature
    }

    public class UserService : ControllerBase, IUserService
    {
        private readonly PhotoFinderContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(PhotoFinderContext context, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> HandleGetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleGetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user by id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
        public async Task<IActionResult> HandleCreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(HandleGetUserById), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new user.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleUpdateUser(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserId);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _context.Entry(existingUser).CurrentValues.SetValues(user); //update all values.
                await _context.SaveChangesAsync();
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user: {Id}", user.UserId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleDeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleGetCurrentUser()
        {
            try
            {
                // 1. Get HttpContext
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    return StatusCode(400, "HttpContext is not available."); // Or throw an exception
                }

                // 2. Get User ID from Claims
                var userIdClaim = httpContext.User.FindFirst("UserId");  // Use the string "UserId"
                if (userIdClaim == null)
                {
                    return Unauthorized("User is not authenticated.");
                }
                var userId = userIdClaim.Value;

                // 3. Retrieve User from Database
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // 4. Return the User
                return Ok(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current user");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
