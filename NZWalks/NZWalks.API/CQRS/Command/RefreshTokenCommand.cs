using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto> {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
