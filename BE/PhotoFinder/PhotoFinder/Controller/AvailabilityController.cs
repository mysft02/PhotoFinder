using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Availability;
using PhotoFinder.DTO.Package;
using PhotoFinder.Infrastructure.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IConfiguration _config;

        public AvailabilityController(IConfiguration config, IAvailabilityService availabilityService)
        {
            _config = config;
            _availabilityService = availabilityService;
        }

        [Authorize]
        [HttpGet("Get_Availability_By_Photographer_Id")]
        public async Task<IActionResult> GetAvailabilityByPhotographerId()
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            var userRole = currentUser.FindFirst("Role")?.Value;
            if(userRole != "photographer")
            {
                return BadRequest("Unauthorized");
            }

            return await _availabilityService.HandleGetAvailabilityByPhotographerId(userId);
        }

        [Authorize]
        [HttpPost("Create_Availability")]
        public async Task<IActionResult> CreateAvailability([FromBody] AvailabilityCreateDTO availabilityCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;
            

            return await _availabilityService.HandleCreateAvailability(availabilityCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Availability")]
        public async Task<IActionResult> UpdateAvailability([FromBody] AvailabilityUpdateDTO availabilityUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _availabilityService.HandleUpdateAvailability(availabilityUpdateDTO, userId);
        }
    }
}
