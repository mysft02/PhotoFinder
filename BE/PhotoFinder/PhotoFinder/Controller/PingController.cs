using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhotoFinder.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace PhotoFinder.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PingController> _logger;
        private readonly PhotoFinderDbContext _dbContext;

        public PingController(IConfiguration config, ILogger<PingController> logger, PhotoFinderDbContext dbContext)
        {
            _config = config;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetPing")]
        public IActionResult Get()
        {
            _logger.LogInformation("Ping");
            var appVer = Environment.GetEnvironmentVariable("APP_VERSION");

            return Ok("Ok");
        }

        [HttpGet("db", Name = "GetPingDb")]
        public IActionResult GetDb()
        {
            _logger.LogInformation("Ping DB");

            try
            {
                _dbContext.Database.OpenConnection();

                return Ok("Connect DB Success");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ping DB failed");

                return BadRequest("Connect DB Failed");
            }
        }
    }
}