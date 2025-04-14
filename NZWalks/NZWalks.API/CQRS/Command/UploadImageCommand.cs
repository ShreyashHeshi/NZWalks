using MediatR;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.CQRS.Command
{
    public class UploadImageCommand: IRequest<ImageUploadRequestDto>
    {
        public ImageUploadRequestDto ImageUploadRequestDto { get; set; }

        public UploadImageCommand(ImageUploadRequestDto ImageUploadRequestDto)
        {
           this.ImageUploadRequestDto = ImageUploadRequestDto;
        }
    }
}
