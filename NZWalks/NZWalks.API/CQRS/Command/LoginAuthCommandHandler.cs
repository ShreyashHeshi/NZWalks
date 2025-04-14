using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace NZWalks.API.CQRS.Command
{
    public class LoginAuthCommandHandler: IRequestHandler<LoginAuthCommand, LoginResponseDto>
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepositary tokenRepositary;

        public LoginAuthCommandHandler(UserManager<IdentityUser> userManager, ITokenRepositary tokenRepositary)
        {
            this.userManager = userManager;
            this.tokenRepositary = tokenRepositary;
            
        }

        public async Task<LoginResponseDto> Handle(LoginAuthCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.LoginRequestDto.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, request.LoginRequestDto.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                var jwtToken = tokenRepositary.CreateJWTToken(user, roles.ToList());
                var refreshToken = tokenRepositary.GenerateRefreshToken();

                await tokenRepositary.StoreRefreshTokenAsync(new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    JwtId = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken).Id,
                    IsUsed = false,
                    IsRevoked = false,
                    AddedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                });

                return new LoginResponseDto
                {
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken,
                    TokenExpiration = DateTime.Now.AddMinutes(15)
                };
            }

            return null;
        }
    }
}
