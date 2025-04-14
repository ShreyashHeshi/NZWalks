namespace NZWalks.API.Models.DTO
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
