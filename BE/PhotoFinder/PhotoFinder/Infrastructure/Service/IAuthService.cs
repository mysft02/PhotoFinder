using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PhotoFinder.Entity;
using System.Security.Claims;
using PhotoFinder.Infrastructure.Database;
using PhotoFinder.DTO.User;

namespace PhotoFinder.Infrastructure.Service
{
    public interface IAuthService
    {
        Task<IActionResult> HandleLogin(UserLoginDTO userLoginDTO);
        Task<IActionResult> HandleRegister(UserRegisterDTO userRegisterDTO);
    }

    public class AuthService : ControllerBase, IAuthService
    {
        private readonly PhotoFinderDbContext _context;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public AuthService(PhotoFinderDbContext context, IMemoryCache cache, IConfiguration config, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
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
                var createdUser = new users();

                var userDuplicate = _context.Users.FirstOrDefault(x => x.email == userRegisterDTO.Email);

                if (userDuplicate != null) { return BadRequest("Email already exists"); }

                createdUser.name = userRegisterDTO.Username;
                createdUser.phone_number = userRegisterDTO.Phone;
                createdUser.email = userRegisterDTO.Email;
                createdUser.password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);
                createdUser.role = userRegisterDTO.Role;

                //var check = true;

                //while (check)
                //{
                //    var id = _jwtService.GenerateId();
                //    var checkId = _context.Users.FirstOrDefault(x => x.user_id == id);
                //    if (checkId == null)
                //    {
                //        createdUser.user_id = id;
                //        check = false;
                //    }
                //}

                createdUser.created_at = DateTime.Now;
                createdUser.updated_at = DateTime.Now;

                _context.Users.Add(createdUser);

                //if (createdUser.Role == "Photographer")
                //{
                //    var createdTherapist = new Photographer
                //    {
                //        UserId = createdUser.Id,
                //        CreatedAt = DateTime.Now,
                //    };

                //    var checkPhoto = true;

                //    while (checkPhoto)
                //    {
                //        var id = Guid.NewGuid();
                //        var checkId = _context.Photographers.FirstOrDefault(x => x.PhotographerId == id);
                //        if (checkId == null)
                //        {
                //            createdTherapist.PhotographerId = id;
                //            checkPhoto = false;
                //        }
                //    }

                //    _context.Photographers.Add(createdTherapist);
                //}

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
                var loginUser = _context.Users.FirstOrDefault(x => x.email == userLoginDTO.Email);

                if (loginUser == null || !BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, loginUser.password))
                {
                    return Unauthorized("Invalid username or password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, loginUser.user_id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, loginUser.name),
                    new Claim(ClaimTypes.Email, loginUser.email),
                    new Claim(ClaimTypes.MobilePhone, loginUser.phone_number),
                    new Claim(ClaimTypes.Role, loginUser.role.ToString())
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
