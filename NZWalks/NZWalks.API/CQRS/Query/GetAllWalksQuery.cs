using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Query
{
    public class GetAllWalksQuery: IRequest<List<WalkDTO>>
    {
        public string? FilterOn { get; set; }
        public string? FilterQuery { get; set; }
        public string? SortBy { get; set; }
        public bool? IsAscending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 1000;

        public GetAllWalksQuery(string? filterOn, string? filterQuery, string? sortBy, bool? isAscending, int pageNumber, int pageSize)
        {
            FilterOn = filterOn;
            FilterQuery = filterQuery;
            SortBy = sortBy;
            IsAscending = isAscending;
            PageNumber = pageNumber;
            PageSize = pageSize;

        }
    }
}
