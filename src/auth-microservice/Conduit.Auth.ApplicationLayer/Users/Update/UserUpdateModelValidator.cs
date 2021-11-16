using System.Text.RegularExpressions;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UserUpdateModelValidator : AbstractValidator<UpdateUserModel>
    {
        private static readonly Regex AcceptedPasswordRegex = new(
            UserModelValidator.AcceptedPasswordRegexPattern,
            RegexOptions.Compiled);

        private static readonly Regex AcceptedUsernameRegex = new(
            UserModelValidator.AcceptedUsernameRegexPattern,
            RegexOptions.Compiled);

        public UserUpdateModelValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.Username)
                .MinimumLength(8)
                .MaximumLength(100)
                .Matches(AcceptedUsernameRegex);
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(100);
            RuleFor(x => x.Biography)
                .MaximumLength(500);
            var imagePropertyValidator =
                new ImagePropertyValidator<UpdateUserModel>(imageChecker);
            RuleFor(x => x.Image)
                .MaximumLength(1000)
                .SetAsyncValidator(imagePropertyValidator);
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(500)
                .Matches(AcceptedPasswordRegex);
        }
    }
}
