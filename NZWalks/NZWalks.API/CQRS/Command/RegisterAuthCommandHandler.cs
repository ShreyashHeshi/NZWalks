using MediatR;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class RegisterAuthCommandHandler : IRequestHandler<RegisterAuthCommand, string>
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepositary tokenRepositary;

        public RegisterAuthCommandHandler(UserManager<IdentityUser> userManager, ITokenRepositary tokenRepositary)
        {
            this.userManager = userManager;
            this.tokenRepositary = tokenRepositary;
        }
        public async Task<string> Handle(RegisterAuthCommand request, CancellationToken cancellationToken)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.RegisterRequestDto.UserName,
                Email = request.RegisterRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, request.RegisterRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // add roles to this user
                if (request.RegisterRequestDto.Roles != null && request.RegisterRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.RegisterRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return "user was registered please login";
                    }
                }
            }

            return "Something went wrong";
        }
    }
}
