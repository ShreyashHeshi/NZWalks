using AutoMapper;
using MediatR;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Query
{
    public class GetWalkByIdQueryHandler : IRequestHandler<GetWalkByIdQuery, WalkDTO?>
    {

        private readonly IWalkRepositary walkRepository;
        private readonly IMapper mapper;

        public GetWalkByIdQueryHandler(IWalkRepositary walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        public async Task<WalkDTO?> Handle(GetWalkByIdQuery request, CancellationToken cancellationToken)
        {
            var walk = await walkRepository.GetByIdAsync(request.Id);
            return walk == null ? null : mapper.Map<WalkDTO>(walk);
        }
    }
}
