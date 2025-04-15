using QRCoder;
using MediatR;
using NZWalks.API.CQRS.Query;
using NZWalks.API.Models.DTO;
using NZWalks.API.Services;


namespace NZWalk.Api.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IMediator mediator;

        public QrCodeService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<RegionQrResponseDto> GenerateQRCodeForRegionAsync(RegionQrRequestDto request)
        {
            var region = await mediator.Send(new GetRegionByQRQuery { RegionName = request.RegionName });

            if (region == null)
                throw new ApplicationException($"Region '{request.RegionName}' not found.");

            var qrContent = $"Region Name: {region.Name} | Code: {region.Code} | Image: {region.RegionImageUrl ?? "N/A"}";

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var imageBytes = qrCode.GetGraphic(request.Size > 0 ? request.Size : 20);

            return new RegionQrResponseDto
            {
                RegionName = region.Name,
                RegionCode = region.Code,
                RegionImageUrl = region.RegionImageUrl,
                QRCodeImageBase64 = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}",
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}