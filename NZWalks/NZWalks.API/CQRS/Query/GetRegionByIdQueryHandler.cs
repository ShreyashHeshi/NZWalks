using MediatR;
using NZWalks.API.Repositories;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.CQRS.Query
{
    public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Region>
    {

        private readonly IRegionRepositary _regionRepository;

        public GetRegionByIdQueryHandler(IRegionRepositary regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Region> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _regionRepository.GetByIdAsync(request.Id);
        }
    }
}
