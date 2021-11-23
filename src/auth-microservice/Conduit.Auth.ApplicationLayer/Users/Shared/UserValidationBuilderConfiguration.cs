using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Domain.Users.Services;
using FluentValidation;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public static class UserValidationBuilderConfiguration
    {
        public static IRuleBuilder<TModel, string?> UniqueEmail<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IUsersFindByEmailRepository findByEmailRepository,
            ICurrentUserProvider? currentUserProvider = null) =>
            ruleBuilder.SetAsyncValidator(new UniqueEmailValidator<TModel>(
                findByEmailRepository,
                currentUserProvider));

        public static IRuleBuilder<TModel, string?> UniqueUsername<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IUsersFindByUsernameRepository findByUsernameRepository,
            ICurrentUserProvider? currentUserProvider = null) =>
            ruleBuilder.SetAsyncValidator(new UniqueUsernameValidator<TModel>(
                findByUsernameRepository,
                currentUserProvider));

        public static IRuleBuilder<TModel, string?> ValidImageUrl<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder,
            IImageChecker imageChecker) =>
            ruleBuilder.MaximumLength(1000)
                .SetAsyncValidator(new ImagePropertyValidator<TModel>(imageChecker));

        public static IRuleBuilder<TModel, string?> ValidUsername<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder) =>
            ruleBuilder
                .MinimumLength(8)
                .MaximumLength(500)
                .Matches(UserValidationConfiguration.AcceptedUsernameRegex);
        
        public static IRuleBuilder<TModel, string?> ValidPassword<TModel>(
            this IRuleBuilder<TModel, string?> ruleBuilder) =>
            ruleBuilder
                .MinimumLength(8)
                .MaximumLength(500)
                .Matches(UserValidationConfiguration.AcceptedPasswordRegex);
    }
}