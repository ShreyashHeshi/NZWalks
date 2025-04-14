using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using MediatR;
using NZWalks.API.Models.DTO;
using AutoMapper;



namespace NZWalks.API.CQRS.Query
{
    public class GetAllRegionsQueryHandler: IRequestHandler<GetAllRegionsQuery, RegionListResponse> // IRequestHandler from MediatR, handles query request and returns list of regions
    {
        private readonly IRegionRepositary regionRepositary;
        private readonly IMapper mapper;
        public GetAllRegionsQueryHandler(IRegionRepositary regionRepositary, IMapper mapper)
        {
            this.regionRepositary = regionRepositary;
            this.mapper = mapper;
        }

        public async Task<RegionListResponse> Handle(GetAllRegionsQuery request, CancellationToken cancellationToken) //If the client cancels the request (e.g., browser stops loading, timeout occurs), the CancelationToken is triggered to stop execution. 
        {
            // Get total count of all regions
            var totalCount = await regionRepositary.GetTotalCountAsync();

            // Get paginated regions
            var regions = await regionRepositary.GetAllAsync(request.Page, request.PageSize);

            // Map to DTO
            var regionDtos = mapper.Map<List<RegionDTO>>(regions);

            return new RegionListResponse
            {
                Regions = regionDtos,
                TotalCount = totalCount
            };
        }

    }
}
