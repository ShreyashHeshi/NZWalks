using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class UpdateWalkCommand: IRequest<WalkDTO>
    {
        public Guid id { get; set; }

        public UpdateWalkRequestDto updateWalkRequestDto { get; set; }

        public UpdateWalkCommand(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            this.id = id;
            this.updateWalkRequestDto = updateWalkRequestDto;
        }
    }
}
