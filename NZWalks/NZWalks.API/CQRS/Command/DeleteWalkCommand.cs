using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class DeleteWalkCommand: IRequest<WalkDTO>
    {
        public Guid id { get; set; }
        public DeleteWalkCommand(Guid id)
        {
            this.id = id;
        }
    }
}
