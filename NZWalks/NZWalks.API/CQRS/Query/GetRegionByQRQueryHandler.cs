using AutoMapper;
using MediatR;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Query
{
    public class GetRegionByQRQueryHandler : IRequestHandler<GetRegionByQRQuery, RegionDTO>
    {
        private readonly IRegionRepositary _repository;
        private readonly IMapper mapper;

        public GetRegionByQRQueryHandler(IRegionRepositary repository, IMapper mapper)
        {
            _repository = repository;
            this.mapper = mapper;
        }

        public async Task<RegionDTO> Handle(GetRegionByQRQuery request, CancellationToken cancellationToken)
        {
            var region = await _repository.GetByNameAsync(request.RegionName);
            if (region == null) return null;

           return mapper.Map<RegionDTO>(region);
        }
    }
}
