using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.CQRS.Command;
using NZWalks.API.Data;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NZWalks.API.CQRS.Command
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly ITokenRepositary tokenRepository;
        private readonly NZWalksAuthDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public RefreshTokenCommandHandler(
            ITokenRepositary tokenRepository,
            NZWalksAuthDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            this.tokenRepository = tokenRepository;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = tokenRepository.GetPrincipalFromExpiredToken(request.AccessToken)
                ?? throw new SecurityTokenException("Invalid access token");

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email)
                ?? throw new SecurityTokenException("User not found");

            var storedRefreshToken = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken && x.UserId == user.Id)
                ?? throw new SecurityTokenException("Refresh token not found");

            if (storedRefreshToken.IsUsed) throw new SecurityTokenException("Refresh token has been used");
            if (storedRefreshToken.IsRevoked) throw new SecurityTokenException("Refresh token has been revoked");
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate) throw new SecurityTokenException("Refresh token has expired");

            var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (storedRefreshToken.JwtId != jti) throw new SecurityTokenException("Refresh token does not match this JWT");

            storedRefreshToken.IsUsed = true;
            dbContext.RefreshTokens.Update(storedRefreshToken);

            var roles = await userManager.GetRolesAsync(user);
            var newAccessToken = tokenRepository.CreateJWTToken(user, roles.ToList());
            var newRefreshToken = tokenRepository.GenerateRefreshToken();

            var newRefreshTokenRecord = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                JwtId = new JwtSecurityTokenHandler().ReadJwtToken(newAccessToken).Id,
                IsUsed = false,
                IsRevoked = false,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await dbContext.RefreshTokens.AddAsync(newRefreshTokenRecord);
            await dbContext.SaveChangesAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiration = DateTime.Now.AddMinutes(15)
            };
        }
    }
}
