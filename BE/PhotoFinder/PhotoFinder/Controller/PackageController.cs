using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.Package;
using PhotoFinder.DTO.Photographer;
using PhotoFinder.Infrastructure.Service;
using System.Security.Claims;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IConfiguration _config;

        public PackageController(IConfiguration config, IPackageService packageService)
        {
            _config = config;
            _packageService = packageService;
        }

        [HttpGet("Get_All_Packages")]
        public async Task<IActionResult> GetAllPackages()
        {

            return await _packageService.HandleGetAllPackages();
        }

        [HttpGet("Get_Package_By_Id")]
        public async Task<IActionResult> GetPackageById([FromQuery] int id)
        {

            return await _packageService.HandleGetPackageById(id);
        }

        [Authorize]
        [HttpPost("Create_Package")]
        public async Task<IActionResult> CreatePackage([FromBody] PackageCreateDTO packageCreateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _packageService.HandleCreatePackage(packageCreateDTO, userId);
        }

        [Authorize]
        [HttpPost("Update_Package")]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageUpdateDTO packageUpdateDTO)
        {
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst(ClaimTypes.Sid)?.Value;

            return await _packageService.HandleUpdatePackage(packageUpdateDTO, userId);
        }
    }
}
