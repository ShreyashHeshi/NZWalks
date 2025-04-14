using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models;
using System.Security.Claims;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepositary
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task StoreRefreshTokenAsync(RefreshToken refreshToken);
    }
}
