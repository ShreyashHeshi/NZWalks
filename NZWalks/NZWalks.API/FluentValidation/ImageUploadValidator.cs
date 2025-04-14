using FluentValidation;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace NZWalks.API.FluentValidation
{
    public class ImageUploadRequestDtoValidator : AbstractValidator<ImageUploadRequestDto>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const long _maxFileSize = 10485760; // 10MB

        public ImageUploadRequestDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required")
                .Must(file => file.Length > 0).WithMessage("File cannot be empty")
                .Must(file => _allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Unsupported file extension. Allowed: .jpg, .jpeg, .png")
                .Must(file => file.Length <= _maxFileSize)
                .WithMessage("File size exceeds 10MB. Please upload a smaller file.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required")
                .MaximumLength(255).WithMessage("FileName must not exceed 255 characters");

            RuleFor(x => x.FileDescription)
                .MaximumLength(500).WithMessage("FileDescription must not exceed 500 characters");
        }
    }
}
