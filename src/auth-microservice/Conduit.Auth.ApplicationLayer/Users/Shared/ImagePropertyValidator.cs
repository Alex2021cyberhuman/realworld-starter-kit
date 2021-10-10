using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class ImagePropertyValidator
        : AsyncPropertyValidator<UserModel, string?>
    {
        private readonly IImageChecker _imageChecker;

        public ImagePropertyValidator(IImageChecker imageChecker)
        {
            _imageChecker = imageChecker;
        }

        public override string Name => nameof(ImagePropertyValidator);

        public override async Task<bool> IsValidAsync(
            ValidationContext<UserModel> context,
            string? value,
            CancellationToken cancellation)
        {
            if (value is null)
                return true;
            if (await _imageChecker.CheckImageAsync(value, cancellation))
                return true;
            context.AddFailure(
                $"Cannot access this url or invalid format: {value}");
            return false;
        }
    }
}
