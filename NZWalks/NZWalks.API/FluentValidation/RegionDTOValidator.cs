using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.FluentValidation
{
    public class RegionDTOValidator: AbstractValidator<AddRegionRequestDto>
    {
        public RegionDTOValidator()
        {
            
                RuleFor(x => x.Code)
                    .NotEmpty().WithMessage("Code is required")
                    .Length(3).WithMessage("Code must be exactly 3 characters long");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

                RuleFor(x => x.RegionImageUrl)
                    .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _) || string.IsNullOrEmpty(url))
                    .WithMessage("RegionImageUrl must be a valid URL");
            

        }
    }
}
