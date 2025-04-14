using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class UpdateWalkCommandHandler : IRequestHandler<UpdateWalkCommand, WalkDTO>
    {
        private readonly IWalkRepositary walkRepositary;
        private readonly IMapper mapper;

        public UpdateWalkCommandHandler(IWalkRepositary walkRepositary, IMapper mapper)
        {
            this.walkRepositary = walkRepositary;
            this.mapper = mapper;
        }
        public async Task<WalkDTO> Handle(UpdateWalkCommand request, CancellationToken cancellationToken)
        {
            // map dto to domain
            var walkDomain = mapper.Map<Walk>(request.updateWalkRequestDto);

            // update walk
            var updateWalk = await walkRepositary.UpdateAsync(request.id, walkDomain);

            // map domain to dto
            if(updateWalk == null)
            {
                return null;
            }

            var walkDto = mapper.Map<WalkDTO>(updateWalk);

            return walkDto;
        }
    }
}
