using Conduit.Auth.ApplicationLayer.Users.Register;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserRequestValidator
        : AbstractValidator<RegisterUserRequest>
    {
        public UpdateUserRequestValidator(IImageChecker imageChecker)
        {
            RuleFor(x => x.User)
                .SetValidator(new UserModelValidator(imageChecker));
        }
    }
}