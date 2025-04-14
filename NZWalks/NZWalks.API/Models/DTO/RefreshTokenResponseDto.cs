namespace NZWalks.API.Models.DTO
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
