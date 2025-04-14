using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class CreateWalkCommandHandler : IRequestHandler<CreateWalkCommand, WalkDTO>
    {
        private readonly IWalkRepositary walkRepositary;
        private readonly IMapper mapper;

        public CreateWalkCommandHandler(IWalkRepositary walkRepositary, IMapper mapper)
        {
            this.walkRepositary = walkRepositary;
            this.mapper = mapper;
        }

        public async Task<WalkDTO> Handle(CreateWalkCommand request, CancellationToken cancellationToken)
        {
            // map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(request.addWalkRequestDto);

            // create database
            var createWalk = await walkRepositary.CreateAsync(walkDomainModel);

            // map domain to dto
            var walkDto = mapper.Map<WalkDTO>(createWalk);

            if(walkDto == null)
            {
                return null;
            }

            return walkDto;
        }
    }
}
