using ClinicAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicAPI.Helpers
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AppUser user)
        {
            // Null safety for required claims
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentException("UserName cannot be null or empty");
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email cannot be null or empty");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "") // Role can be optional
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new ArgumentException("JWT key is missing in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var expireDaysConfig = _config["Jwt:ExpireDays"];
            if (string.IsNullOrWhiteSpace(expireDaysConfig))
                throw new ArgumentException("ExpireDays is missing in configuration");

            if (!double.TryParse(expireDaysConfig, out double expireDays))
                throw new ArgumentException("ExpireDays must be a valid number");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(expireDays),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
