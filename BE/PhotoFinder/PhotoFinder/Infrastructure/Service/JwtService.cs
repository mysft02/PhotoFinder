using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PhotoFinder.Infrastructure.Service
{
    public class JwtService
    {
        private readonly string DEFAULT_SECRET = "PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f";
        private readonly byte[] _key;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            var SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? DEFAULT_SECRET;
            _key = Encoding.ASCII.GetBytes(SecretKey);
            _handler = new JwtSecurityTokenHandler();
            _config = config;

        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public int GenerateId()
        {
            Random random = new Random();
            int randomInt = random.Next(100000000, 1000000000);

            return randomInt;
        }
    }
}
