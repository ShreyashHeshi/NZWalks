using MediatR;
using NZWalks.API.Models.DTO;



namespace NZWalks.API.CQRS.Command
{
    public class DeleteRegionCommand: IRequest<RegionDTO>
    {
        public Guid Id { get; set; }

        public DeleteRegionCommand(Guid Id)
        {
            this.Id = Id;
        }

    }
}
