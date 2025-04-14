using AutoMapper;
using MediatR;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.CQRS.Command
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ImageUploadRequestDto>
    {
        private readonly IImageRepositary imageRepositary;
        private readonly IMapper mapper;

        public UploadImageCommandHandler(IImageRepositary imageRepositary, IMapper mapper)
        {
            this.imageRepositary = imageRepositary;
            this.mapper = mapper;
        }

        public async Task<ImageUploadRequestDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            // covert DTO to Domain Model
            var imageDomainModel = new Images
            {
                File = request.ImageUploadRequestDto.File,
                FileExtension = Path.GetExtension(request.ImageUploadRequestDto.File.FileName), //Path from System.io.Path
                FileSizeInBytes = request.ImageUploadRequestDto.File.Length,
                FileName = request.ImageUploadRequestDto.FileName,
                FileDescription = request.ImageUploadRequestDto.FileDescription,
            };

            var uploadImage = await imageRepositary.Upload(imageDomainModel);
            if(uploadImage == null)
            {
                return null;
            }

            return new ImageUploadRequestDto
            {
                FileName = uploadImage.FileName,
                FileDescription = uploadImage.FileDescription,
                
                
            };
        }
    }
}
