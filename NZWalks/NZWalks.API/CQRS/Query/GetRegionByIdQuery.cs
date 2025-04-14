using MediatR;
using NZWalks.API.Models.Domain;
using System;



namespace NZWalks.API.CQRS.Query
{
    public class GetRegionByIdQuery : IRequest<Region>
    {
        public Guid Id { get; }
        public GetRegionByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
