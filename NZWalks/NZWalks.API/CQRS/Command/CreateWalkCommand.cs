using MediatR;
using NZWalks.API.Models.DTO;





namespace NZWalks.API.CQRS.Command
{
    public class CreateWalkCommand: IRequest<WalkDTO>
    {
        
        public AddWalkRequestDto addWalkRequestDto {  get; set; }

        public CreateWalkCommand( AddWalkRequestDto addWalkRequestDto)
        {
           
            this.addWalkRequestDto = addWalkRequestDto;
        }
    }
}
