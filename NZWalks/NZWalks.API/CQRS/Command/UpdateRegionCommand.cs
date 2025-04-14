using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class UpdateRegionCommand: IRequest<RegionDTO>
    {
       
        public Guid id { get; set; }

        public UpdateRegionRequestDto updateRegionRequestDto {  get; set; }
        public UpdateRegionCommand(Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {
            this.id = id;
            this.updateRegionRequestDto = updateRegionRequestDto;
        }
    }
}
