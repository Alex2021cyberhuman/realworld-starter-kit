using System.Text.RegularExpressions;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        private const string AcceptedPasswordRegexPattern =
            @"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z])$";


        private const string AcceptedUsernameRegexPattern =
            @"^(?![0-9-._])[a-zA-Z0-9@._-]+(?<![_.-])$";

        private static readonly Regex AcceptedPasswordRegex = new(
            AcceptedPasswordRegexPattern,
            RegexOptions.Compiled);

        private static readonly Regex AcceptedUsernameRegex = new(
            AcceptedUsernameRegexPattern,
            RegexOptions.Compiled);

        public UserModelValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches(AcceptedUsernameRegex);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.Biography).MaximumLength(500);
            var imagePropertyValidator =
                new ImagePropertyValidator<UserModel>(imageChecker);
            RuleFor(x => x.Image)
                .MaximumLength(1000)
                .SetAsyncValidator(imagePropertyValidator);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(500)
                .Matches(AcceptedPasswordRegex);
        }
    }
}
