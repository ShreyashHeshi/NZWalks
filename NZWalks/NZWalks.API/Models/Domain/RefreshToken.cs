// Models/RefreshToken.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string JwtId { get; set; } // Map the token with jwtId

        [Required]
        public bool IsUsed { get; set; } // If used it can't be used again

        [Required]
        public bool IsRevoked { get; set; } // If revoked, it can't be used

        public DateTime AddedDate { get; set; }

        public DateTime ExpiryDate { get; set; } // Refresh token expiry date

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
