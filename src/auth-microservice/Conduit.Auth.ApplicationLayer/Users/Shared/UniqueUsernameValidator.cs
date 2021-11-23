using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UniqueUsernameValidator<T> : AsyncPropertyValidator<T, string?>
    {
        private readonly IUsersFindByUsernameRepository _usernameRepository;
        private readonly ICurrentUserProvider? _currentUserProvider;

        public UniqueUsernameValidator(
            IUsersFindByUsernameRepository usernameRepository,
            ICurrentUserProvider? currentUserProvider = null)
        {
            _usernameRepository = usernameRepository;
            _currentUserProvider = currentUserProvider;
        }
        
        public override async Task<bool> IsValidAsync(
            ValidationContext<T> context,
            string? value,
            CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            var user = await _usernameRepository.FindByUsernameAsync(value, cancellation);
            
            return await user.CheckCurrentUser(
                _currentUserProvider,
                cancellation);
        }

        public override string Name => nameof(UniqueUsernameValidator<T>);
    }
}