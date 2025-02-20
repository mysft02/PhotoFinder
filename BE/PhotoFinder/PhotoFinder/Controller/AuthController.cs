using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFinder.DTO.User;
using PhotoFinder.Infrastructure.Service;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;

        public AuthController(IConfiguration config, JwtService jwtService, IAuthService authService)
        {
            _config = config;
            _jwtService = jwtService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Register([FromBody] UserLoginDTO userLoginDTO)
        {

            return await _authService.HandleLogin(userLoginDTO);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Login([FromBody] UserRegisterDTO userRegisterDTO)
        {
            return await _authService.HandleRegister(userRegisterDTO);
        }
    }
}
