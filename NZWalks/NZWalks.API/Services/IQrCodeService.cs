using NZWalks.API.Models.DTO;

namespace NZWalks.API.Services
{
    public interface IQrCodeService
    {
        Task<RegionQrResponseDto> GenerateQRCodeForRegionAsync(RegionQrRequestDto request);
    }
}
