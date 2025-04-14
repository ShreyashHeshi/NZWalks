using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, RegionDTO>
    {
        private readonly IRegionRepositary regionRepositary;
        private readonly IMapper mapper;

        public UpdateRegionCommandHandler(IRegionRepositary regionRepositary, IMapper mapper)
        {
            this.regionRepositary = regionRepositary;
            this.mapper = mapper;
        }
        public async Task<RegionDTO> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            // map dto to domain model
            var regionDomainModel = mapper.Map<Region>(request.updateRegionRequestDto);

            // update database
            var updateRegion = await
                regionRepositary.UpdateAsync(request.id, regionDomainModel);

            //return mapper.Map<RegionDTO>(updateRegion);

            return updateRegion != null ? mapper.Map<RegionDTO>(updateRegion) : null;






        }
    }
}
