using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Query
{
    public class GetWalkByIdQuery: IRequest<WalkDTO?>
    {
        public Guid Id { get; set; }

        public GetWalkByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
