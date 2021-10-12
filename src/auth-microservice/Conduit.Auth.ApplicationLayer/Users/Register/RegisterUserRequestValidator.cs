using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class RegisterUserRequestValidator
        : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator(IImageChecker imageChecker, IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.User)
                .SetValidator(new UserModelValidator(imageChecker));
            var uniqueEmailValidator = new UniqueEmailValidator<RegisterUserRequest>(unitOfWork);
            var uniqueUsernameValidator = new UniqueUsernameValidator<RegisterUserRequest>(unitOfWork);
            RuleFor(x => x.User.Email)
                .SetAsyncValidator(uniqueEmailValidator)
                .SetAsyncValidator(uniqueUsernameValidator);
        }
    }
}
