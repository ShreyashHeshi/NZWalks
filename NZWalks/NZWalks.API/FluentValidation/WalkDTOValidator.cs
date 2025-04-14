using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.FluentValidation
{
    public class WalkDTOValidator: AbstractValidator<AddWalkRequestDto>
    {
        public WalkDTOValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required")
               .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Length must be greater than 0");

            RuleFor(x => x.LenghtInKm)
                .NotNull().WithMessage("Length is required")
                .InclusiveBetween(0, 100).WithMessage("LengthInKm must be between 0 and 100");

            RuleFor(x => x.DifficultyId)
                 .NotEmpty().WithMessage("DifficultyId is required");

            RuleFor(x => x.RegionId)
                .NotEmpty().WithMessage("RegionId is required");

        }
    }
}
