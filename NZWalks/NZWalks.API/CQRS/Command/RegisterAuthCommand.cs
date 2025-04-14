using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class RegisterAuthCommand: IRequest<string>
    {
        public RegisterRequestDto RegisterRequestDto { get; set; }

        public RegisterAuthCommand(RegisterRequestDto RegisterRequestDto)
        {
            this.RegisterRequestDto = RegisterRequestDto;
        }
    }
}
