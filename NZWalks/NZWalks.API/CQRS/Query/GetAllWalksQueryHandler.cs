using AutoMapper;
using MediatR;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Query
{
    public class GetAllWalksQueryHandler : IRequestHandler<GetAllWalksQuery, List<WalkDTO>>
    {
        private readonly IWalkRepositary walkRepositary;
        private readonly IMapper mapper;

        public GetAllWalksQueryHandler(IWalkRepositary walkRepositary, IMapper mapper)
        {
            this.walkRepositary = walkRepositary;
            this.mapper = mapper;
        }
        public async Task<List<WalkDTO>> Handle(GetAllWalksQuery request, CancellationToken cancellationToken)
        {
            
            var allWalks = await walkRepositary.GetAllAsync(
                request.FilterOn,
                request.FilterQuery,
                request.SortBy,
                request.IsAscending ?? true,  // Default to ascending if null
                request.PageNumber,
                request.PageSize
                );
            return mapper.Map<List<WalkDTO>>(allWalks);
        }
    }
}
