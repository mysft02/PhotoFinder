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

        [HttpGet("Get_Booking")]
        public async Task<IActionResult> GetBooking()
        {

            return await _dashboardService.HandleGetBooking();
        }
    }
}
