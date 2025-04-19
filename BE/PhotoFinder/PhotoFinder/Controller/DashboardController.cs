using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.Infrastructure.Service;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IConfiguration _config;

        public DashboardController(IConfiguration config, IDashboardService dashboardService)
        {
            _config = config;
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("Get_User_Booking_Analytics")]
        public async Task<IActionResult> GetUserBookingAnalytics()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _dashboardService.HandleGetUserBookingAnalytics(int.Parse(userId));
        }

        [Authorize]
        [HttpGet("Get_Photographer_Booking_Analytics")]
        public async Task<IActionResult> GetPhotographerBookingAnalytics()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _dashboardService.HandleGetPhotographerBookingAnalytics(int.Parse(userId));
        }
    }
}
