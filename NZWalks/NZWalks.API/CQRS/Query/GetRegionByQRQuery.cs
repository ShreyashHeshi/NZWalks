using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Query
{
    public class GetRegionByQRQuery: IRequest<RegionDTO>
    {
        public string RegionName { get; set; } = string.Empty;
    }
}
