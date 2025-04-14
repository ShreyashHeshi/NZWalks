using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using NZWalks.API.Models;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;

namespace NZWalks.API.Repositories
{
    public class TokenRepositary : ITokenRepositary
    {
        private readonly IConfiguration configuration;
        private readonly NZWalksAuthDbContext nZWalksAuthDbContext;

        public TokenRepositary(IConfiguration configuration, NZWalksAuthDbContext nZWalksAuthDbContext) // using Iconfiguration we can acces the appsetting
        {
            this.configuration = configuration;
            this.nZWalksAuthDbContext = nZWalksAuthDbContext;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {


            // create claims from System.Security.Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }   

            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);   // this instantiates new jwt token handler and takes token and write token for us
            // writetoken method converts jwtsecuritytoken object into final string representation of jwt

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task StoreRefreshTokenAsync(RefreshToken refreshToken)
        {
            await nZWalksAuthDbContext.RefreshTokens.AddAsync(refreshToken);
            await nZWalksAuthDbContext.SaveChangesAsync();
        }
    }
}
