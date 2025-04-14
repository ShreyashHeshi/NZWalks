using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Query
{
    public class GetAllRegionsQuery: IRequest<RegionListResponse> // implements IRequest which is from MediatR and tells this is Query 
                                                            // and response is list of regions
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }

    public class RegionListResponse
    {
        public List<RegionDTO> Regions { get; set; }
        public int TotalCount { get; set; }
    }
}
