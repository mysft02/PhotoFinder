using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.VNPay;
using PhotoFinder.Infrastructure.Service;
using System.Security.Claims;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IVNPayService _vnPayService;

        public VNPayController(IConfiguration config, IVNPayService vnPayService)
        {
            _config = config;
            _vnPayService = vnPayService;
        }

        // POST: auth/login
        [Authorize]
        [HttpPost("Get-Payment-Url")]
        public async Task<IActionResult> GetVNPayUrl([FromBody] VnPayRequestDTO vnPayRequestDTO)
        {
            var currentUser = HttpContext.User;
            var currentUserId = currentUser.FindFirst("UserId")?.Value;
            var url = HttpContext.Request.GetDisplayUrl();

            return await _vnPayService.HandleCreateVNPayUrl(HttpContext, vnPayRequestDTO, currentUserId, url);
        }

        [AllowAnonymous]
        [HttpPost("Process-Payment")]
        public async Task<IActionResult> ProcessVNPay()
        {
            var url = HttpContext.Request.GetDisplayUrl();
            var currentUser = HttpContext.User;
            var userId = currentUser.FindFirst("UserId")?.Value;

            return await _vnPayService.HandleVNPay(url, userId);
        }
    }
}
