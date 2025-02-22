using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PhotoFinder.Entity;
using System.Security.Claims;
using PhotoFinder.Infrastructure;
using PhotoFinder.DTO.User;
using PhotoFinder.DTO.Photographer;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IAuthService
    {
        Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO);
        Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO);
    }

    public class AuthService : ControllerBase, IAuthService
    {
        private readonly PhotoFinderContext _context;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public AuthService(PhotoFinderContext context, IMemoryCache cache, IConfiguration config, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = config;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var createdUser = new User();

                var userDuplicate = _context.Users.FirstOrDefault(x => x.Email == userRegisterDTO.Email);

                if (userDuplicate != null) { return BadRequest("Email already exists"); }

                createdUser.Name = userRegisterDTO.Username;
                createdUser.PhoneNumber = userRegisterDTO.Phone;
                createdUser.Email = userRegisterDTO.Email;
                createdUser.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                createdUser.Role = userRegisterDTO.Role;
                createdUser.CreatedAt = DateTime.Now;
                createdUser.UpdatedAt = DateTime.Now;

                _context.Users.Add(createdUser);
                await _context.SaveChangesAsync();

                if (createdUser.Role == "photographer")
                {
                    var photographer = new Photographer
                    {
                        UserId = createdUser.UserId,
                        Bio = "",
                        PortfolioUrl = "",
                        Rating = 0,
                        Location = "",
                        CreatedAt = DateTime.Now
                    };

                    _context.Photographers.Add(photographer);
                }

                await _context.SaveChangesAsync();

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(createdUser);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO)
        {
            try
            {
                var loginUser = _context.Users.FirstOrDefault(x => x.Email == userLoginDTO.Email);

                if (loginUser == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, loginUser.Password))
                {
                    return Unauthorized("Invalid username or password");
                }

                var claims = new List<Claim>
                {
                    new Claim("UserId", loginUser.UserId.ToString()),
                    new Claim("Email", loginUser.Email),
                    new Claim("Role", loginUser.Role)
                };

                // Tạo access token
                var accessToken = _jwtService.GenerateAccessToken(claims);

                // Tạo refresh token
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Lưu refresh token vào cache
                //_cache.Set($"RefreshToken_{loginUser.user_id}", refreshToken, TimeSpan.FromDays(7));

                // Đặt refresh token vào cookie
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                var user = new UserLoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                // Trả về thông tin người dùng mới đã đăng ký
                return Ok(user);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
