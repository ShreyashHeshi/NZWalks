using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IPdfGenerator
    {
        Task<byte[]> GenerateRegionsPdfAsync(List<RegionDTO> regions);
        Task<byte[]> GenerateCustomPdfAsync(CustomPdfRequestDto request);
    }
}
