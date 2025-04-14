using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class DeleteWalkCommandHandler : IRequestHandler<DeleteWalkCommand, WalkDTO>
    {
        private readonly IWalkRepositary walkRepositary;
        private readonly IMapper mapper;

        public DeleteWalkCommandHandler(IWalkRepositary walkRepositary, IMapper mapper)
        {
            this.walkRepositary = walkRepositary;
            this.mapper = mapper;
        }

        public async Task<WalkDTO> Handle(DeleteWalkCommand request, CancellationToken cancellationToken)
        {
            // delete walk
            var deletedWalk = await walkRepositary.DeleteAsync(request.id);
            if(deletedWalk == null)
            {
                return null;
            }
            return mapper.Map<WalkDTO>(deletedWalk);

         }
    }
}
