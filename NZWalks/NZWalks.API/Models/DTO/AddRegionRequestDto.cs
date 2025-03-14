using System.ComponentModel.DataAnnotations;


namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be min of 3 char")]
        [MaxLength(3, ErrorMessage = "Code has to be max of 3 char")]// comes from system.componentModel.DataAnnotations
        public string Code { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Code has to be max of 50 char")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
