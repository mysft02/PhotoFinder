using Microsoft.AspNetCore.Mvc;
using PhotoFinder.Infrastructure.Service; // Make sure this namespace is correct
using PhotoFinder.Entity; // Make sure this namespace is correct
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Import for authorization
using Microsoft.AspNetCore.Http;

namespace PhotoFinder.Controllers // Adjust the namespace if needed
{
    [ApiController]
    [Route("api/users")] // Define the base route for this controller
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor; // Inject HttpContextAccessor

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor; // Initialize
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userService.HandleGetAllUsers();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return await _userService.HandleGetUserById(id);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _userService.HandleCreateUser(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != user.UserId) // Ensure the ID in the route matches the ID in the body
            {
                return BadRequest("ID mismatch");
            }
            return await _userService.HandleUpdateUser(user);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userService.HandleDeleteUser(id);
        }

        // GET: api/users/current
        [HttpGet("current")]
        [Authorize] // Requires authentication
        public async Task<IActionResult> GetCurrentUser()
        {
            // No need to get user ID from route, service gets it from HttpContext
            return await _userService.HandleGetCurrentUser();
        }
    }
}
