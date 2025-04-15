namespace NZWalks.API.Models.DTO
{
    public class RegionQrResponseDto
    {
        public string RegionName { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
        public string? RegionImageUrl { get; set; }
        public string QRCodeImageBase64 { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }
}
