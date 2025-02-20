using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Infrastructure.Service;
using System.Security.Claims;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotographerController : ControllerBase
    {
        private readonly IPhotographerService _photographerService;
        private readonly IConfiguration _config;

        public PhotographerController(IConfiguration config, IPhotographerService photographerService)
        {
            _config = config;
            _photographerService = photographerService;
        }

        [HttpGet("Get_All_Photographers")]
        public async Task<IActionResult> GetAllPhotographers()
        {

            return await _photographerService.HandleGetAllPhotographers();
        }

        [HttpGet("Get_Photographer_By_Id")]
        public async Task<IActionResult> GetPhotographerById([FromQuery] int id)
        {

            return await _photographerService.HandleGetPhotographerById(id);
        }

        //[Authorize]
        //[HttpPost("Create_Photographer")]
        //public async Task<IActionResult> CreatePhotographer([FromBody] PhotographerCreateDTO photographerCreateDTO)
        //{
        //    var currentUser = HttpContext.User;
        //    var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

        //    return await _photographerService.HandleCreatePhotographer(photographerCreateDTO, userId);
        //}

        [Authorize]
        [HttpPost("Update_Photographer")]
        public async Task<IActionResult> UpdatePhotographer([FromBody] PhotographerUpdateDTO photographerUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _photographerService.HandleUpdatePhotographer(photographerUpdateDTO, userId);
        }
    }
}
