using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestValidator
        : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.User)
                .SetValidator(new UserModelValidator(imageChecker));
        }
    }
}
