using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class LoginAuthCommand: IRequest<LoginResponseDto>
    {
        public LoginRequestDto LoginRequestDto { get; set; }
        public LoginAuthCommand(LoginRequestDto LoginRequestDto)
        {
            this.LoginRequestDto = LoginRequestDto;
        }

    }
}
