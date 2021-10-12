using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using FluentValidation;
using FluentValidation.Validators;

namespace Conduit.Auth.ApplicationLayer.Users.Register
{
    public class UniqueUsernameValidator<T> : AsyncPropertyValidator<T, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UniqueUsernameValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public override async Task<bool> IsValidAsync(
            ValidationContext<T> context,
            string value,
            CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            var user = await _unitOfWork.FindUserByUsernameAsync(value, cancellation);
            return user is null;
        }

        public override string Name => nameof(UniqueUsernameValidator<T>);
    }
}