using Conduit.Auth.ApplicationLayer.Users.Register;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Update
{
    public class UpdateUserRequestValidator
        : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator(
            IImageChecker imageChecker,
            IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.User)
                .SetValidator(new UserUpdateModelValidator(imageChecker));
            var uniqueEmailValidator =
                new UniqueEmailValidator<UpdateUserRequest>(unitOfWork);
            var uniqueUsernameValidator =
                new UniqueUsernameValidator<UpdateUserRequest>(unitOfWork);
            RuleFor(x => x.User.Email)
                .SetAsyncValidator(uniqueEmailValidator!);
            RuleFor(x => x.User.Username)
                .SetAsyncValidator(uniqueUsernameValidator!);
        }
    }
}
