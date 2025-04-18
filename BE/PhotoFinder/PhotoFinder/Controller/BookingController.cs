using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Booking; // Make sure your DTOs are in the correct namespace
using PhotoFinder.Infrastructure.Service; // And your service
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Import for authorization if needed
using System.Security.Claims;
using PhotoFinder.Entity;

namespace PhotoFinder.Controllers // Correct namespace
{
    [ApiController]
    [Route("api/bookings")] // Define your route
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService; 
        public BookingController(IBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            return await _bookingService.HandleGetAllBookings();
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            return await _bookingService.HandleGetBookingById(id);
        }

        // POST: api/bookings
        [HttpPost]
       // [Authorize] // Requires the user to be authenticated
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Handle validation errors
            }
            // Get the user's ID.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _bookingService.HandleCreateBooking(createBookingDTO, userId);
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
      //  [Authorize] // Requires the user to be authenticated
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDTO updateBookingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Handle validation errors
            }
            if (id != updateBookingDTO.BookingId)
            {
                return BadRequest("ID mismatch"); // Ensure the ID in the route matches the ID in the DTO
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _bookingService.HandleUpdateBooking(updateBookingDTO, userId);
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
      //  [Authorize] // Requires the user to be authenticated
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _bookingService.HandleDeleteBooking(id, userId);
        }

        // GET: api/bookings/user
        [HttpGet("user")]
        //[Authorize] // Requires the user to be authenticated
        public async Task<IActionResult> GetBookingsByUser()
        {
            return await _bookingService.HandleGetBookingsByUserId();
        }
    }
}

