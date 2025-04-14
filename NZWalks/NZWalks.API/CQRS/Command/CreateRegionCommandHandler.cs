using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, Region>
    {
        private readonly IRegionRepositary regionRepositary;
        private readonly IMapper mapper;

        public CreateRegionCommandHandler(IRegionRepositary regionRepositary, IMapper mapper)
        {
            this.regionRepositary = regionRepositary;
            this.mapper = mapper;
        }

        public async Task<Region> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = new Region
            {
                Code = request.Code,
                Name = request.Name,
                RegionImageUrl = request.RegionImageUrl
            };

            return await regionRepositary.CreateAsync(region);





            /*// CreateRegionCommand into Region
            var region = mapper.Map<Region>(request);

            // Save to Database
            var createdRegion = await regionRepositary.CreateAsync(region);


            return createdRegion;*/



        }
    }
}
