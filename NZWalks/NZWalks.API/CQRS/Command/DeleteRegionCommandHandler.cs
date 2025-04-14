using AutoMapper;
using MediatR;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, RegionDTO>
    {
        private readonly IRegionRepositary regionRepositary;
        private readonly IMapper mapper;

        public DeleteRegionCommandHandler(IRegionRepositary regionRepositary, IMapper mapper)
        {
            this.regionRepositary = regionRepositary;
            this.mapper = mapper;
        }


        public async Task<RegionDTO> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            var deletedRegion = await regionRepositary.DeleteAsync(request.Id);

            if (deletedRegion == null)
            {
                return null;
            }

            return mapper.Map<RegionDTO>(deletedRegion);
        }
    }
}
