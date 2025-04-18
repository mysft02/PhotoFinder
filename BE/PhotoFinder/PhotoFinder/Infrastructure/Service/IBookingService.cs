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
using Microsoft.AspNetCore.Http;

namespace PhotoFinder.Infrastructure.Service
{
    // 1.  Define the Enum for Booking Status
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public interface IBookingService
    {
        Task<IActionResult> HandleCreateBooking(CreateBookingDTO createBookingDTO, string? userId);
        Task<IActionResult> HandleGetAllBookings();
        Task<IActionResult> HandleGetBookingById(int id);
        Task<IActionResult> HandleUpdateBooking(UpdateBookingDTO updateBookingDTO, string? userId);
        Task<IActionResult> HandleDeleteBooking(int id, string? userId);
        Task<IActionResult> HandleGetBookingsByUserId();
    }

    public class BookingService : ControllerBase, IBookingService
    {
        private readonly PhotoFinderContext _context;
        private readonly ILogger<BookingService> _logger;
        private readonly IUserService _userService; // Inject UserService
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(PhotoFinderContext context, ILogger<BookingService> logger, IUserService userService, IHttpContextAccessor httpContextAccessor)  //add IUserService
        {
            _context = context;
            _logger = logger;
            _userService = userService; // Initialize UserService
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> HandleGetAllBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Select(b => new BookingDTO
                    {
                        BookingId = b.BookingId,
                        CustomerId = b.CustomerId,
                        PhotographerId = b.PhotographerId,
                        EventDate = b.EventDate,
                        EventLocation = b.EventLocation,
                        Status = b.Status,
                        TotalPrice = b.TotalPrice,
                        CreatedAt = b.CreatedAt,
                        UpdatedAt = b.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all bookings");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleGetBookingById(int id)
        {
            try
            {
                var booking = await _context.Bookings
                   .Select(b => new BookingDTO
                   {
                       BookingId = b.BookingId,
                       CustomerId = b.CustomerId,
                       PhotographerId = b.PhotographerId,
                       EventDate = b.EventDate,
                       EventLocation = b.EventLocation,
                       Status = b.Status,
                       TotalPrice = b.TotalPrice,
                       CreatedAt = b.CreatedAt,
                       UpdatedAt = b.UpdatedAt
                   })
                    .FirstOrDefaultAsync(b => b.BookingId == id);

                if (booking == null)
                {
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting booking by id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleCreateBooking(CreateBookingDTO createBookingDTO, string? userId)
        {
            try
            {
                // Get the current user's ID
                var currentUserResult = await _userService.HandleGetCurrentUser(); //use await
                if (currentUserResult is OkObjectResult okResult)
                {
                    var user = okResult.Value as User; // Assuming HandleGetCurrentUser returns a User object
                    if (user != null)
                    {
                        createBookingDTO.CustomerId = user.UserId; // Set CustomerId
                    }
                    else
                    {
                        _logger.LogError("Current User is null");
                        return BadRequest("Current User is null");
                    }
                }
                else if (currentUserResult is BadRequestObjectResult badRequestResult)
                {
                    _logger.LogError($"Bad Request: {badRequestResult.Value}");
                    return BadRequest(badRequestResult.Value); // Return the error from GetCurrentUser
                }
                else if (currentUserResult is UnauthorizedObjectResult unauthorizedResult)
                {
                    _logger.LogError($"Unauthorized: {unauthorizedResult.Value}");
                    return Unauthorized(unauthorizedResult.Value);
                }
                else
                {
                    _logger.LogError("Failed to retrieve current user");
                    return StatusCode(500, "Failed to retrieve current user");
                }

                var booking = new Booking
                {
                    CustomerId = createBookingDTO.CustomerId,
                    PhotographerId = createBookingDTO.PhotographerId,
                    EventDate = createBookingDTO.EventDate,
                    EventLocation = createBookingDTO.EventLocation,
                    Status = createBookingDTO.Status, // Use the string from the DTO
                    TotalPrice = createBookingDTO.TotalPrice,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new booking.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleUpdateBooking(UpdateBookingDTO updateBookingDTO, string? userId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(updateBookingDTO.BookingId);
                if (booking == null)
                {
                    return NotFound();
                }

                // Get the current user's ID.
                var currentUserResult = await _userService.HandleGetCurrentUser(); //use await
                if (currentUserResult is OkObjectResult okResult)
                {
                    var user = okResult.Value as User;  // Assuming HandleGetCurrentUser returns a User
                    if (user != null)
                    {
                        if (updateBookingDTO.CustomerId != user.UserId)
                        {
                            return Unauthorized("You are not authorized to update this booking.");
                        }
                    }
                    else
                    {
                        _logger.LogError("Current User is null");
                        return BadRequest("Current User is null");
                    }
                }
                else if (currentUserResult is BadRequestObjectResult badRequestResult)
                {
                    _logger.LogError($"Bad Request: {badRequestResult.Value}");
                    return BadRequest(badRequestResult.Value); // Return the error from GetCurrentUser
                }
                else if (currentUserResult is UnauthorizedObjectResult unauthorizedResult)
                {
                    _logger.LogError($"Unauthorized: {unauthorizedResult.Value}");
                    return Unauthorized(unauthorizedResult.Value);
                }
                else
                {
                    _logger.LogError("Failed to retrieve current user");
                    return StatusCode(500, "Failed to retrieve current user");
                }
                // Only update fields that are provided in the DTO
                if (updateBookingDTO.CustomerId.HasValue)
                    booking.CustomerId = updateBookingDTO.CustomerId.Value;
                if (updateBookingDTO.PhotographerId.HasValue)
                    booking.PhotographerId = updateBookingDTO.PhotographerId.Value;
                if (updateBookingDTO.EventDate.HasValue)
                    booking.EventDate = updateBookingDTO.EventDate.Value;
                if (updateBookingDTO.EventLocation != null)
                    booking.EventLocation = updateBookingDTO.EventLocation;
                if (updateBookingDTO.Status != null)
                    booking.Status = updateBookingDTO.Status; // Use the string from the DTO
                if (updateBookingDTO.TotalPrice.HasValue)
                    booking.TotalPrice = updateBookingDTO.TotalPrice.Value;
                booking.UpdatedAt = DateTime.UtcNow;

                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating booking: {Id}", updateBookingDTO.BookingId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleDeleteBooking(int id, string? userId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public async Task<IActionResult> HandleGetBookingsByUserId()
        {

            try
            {
                var currentUserResult = await _userService.HandleGetCurrentUser();
                if (currentUserResult is OkObjectResult okResult)
                {
                    var user = okResult.Value as User;
                    if (user == null)
                    {
                        _logger.LogError("Current User is null");
                        return BadRequest("Current User is null");
                    }

                    int userId = user.UserId; // Use string, and get it from the User object.


                    var bookings = await _context.Bookings
                        .Where(b => b.CustomerId == userId)
                        .Select(b => new BookingDTO
                        {
                            BookingId = b.BookingId,
                            CustomerId = b.CustomerId,
                            PhotographerId = b.PhotographerId,
                            EventDate = b.EventDate,
                            EventLocation = b.EventLocation,
                            Status = b.Status,
                            TotalPrice = b.TotalPrice,
                            CreatedAt = b.CreatedAt,
                            UpdatedAt = b.UpdatedAt
                        })
                        .ToListAsync();

                    if (bookings == null || bookings.Count == 0)
                    {
                        return NotFound();
                    }

                    return Ok(bookings);
                }
                else if (currentUserResult is BadRequestObjectResult badRequestResult)
                {
                    return BadRequest(badRequestResult.Value);
                }
                else if (currentUserResult is UnauthorizedObjectResult unauthorizedResult)
                {
                    return Unauthorized(unauthorizedResult.Value);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error"); // Handle other cases.
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting bookings for current user");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }

} 



