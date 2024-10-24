
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.Helpers;
using backend.Interface;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class JWTServices : IJWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public readonly DBContext _context;
        public JWTServices(
            IConfiguration config,
            DBContext context
            )
        {
            _context = context;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        }


        public string CreateJwtTokenAsync(User user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("IsOrganizer", user.UserType.ToString()),
                new Claim("IsAdmin", user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                // new Claim (JwtRegisteredClaimNames.Email, appUser.Email)
            };
            if (user.UserType == UserType.Admin)
            {
                claims.Add(new Claim("IsAdmin", "True"));
            }
            else if (user.UserType == UserType.Organizer)
            {
                claims.Add(new Claim("IsOrganizer", "True"));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(5),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int GenerateToken()
        {
            Random random = new Random();
            return random.Next(100000, 1000000);
        }

        public string RandomPassword()
        {
            Random random = new Random();
            char capitalLetter = (char)random.Next('A', 'Z' + 1);
            char digit = (char)random.Next('0', '9' + 1);
            char[] lowercaseLetters = new char[6];
            for (int i = 0; i < lowercaseLetters.Length; i++)
            {
                lowercaseLetters[i] = (char)random.Next('a', 'z' + 1);
            }
            string password = capitalLetter.ToString() + digit + new string(lowercaseLetters);
            return new string(password.OrderBy(c => random.Next()).ToArray());
        }

        
    }
}