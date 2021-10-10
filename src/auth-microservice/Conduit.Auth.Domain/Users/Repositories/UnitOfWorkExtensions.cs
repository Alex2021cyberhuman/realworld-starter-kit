using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;

namespace Conduit.Auth.Domain.Users.Repositories
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<User> CreateUserAsync(
            this IUnitOfWork unitOfWork,
            User newUser,
            CancellationToken cancellationToken = default)
        {
            var repository =
                unitOfWork.GetRequiredRepository<IUsersWriteRepository>();
            var user = await repository.CreateAsync(newUser, cancellationToken);
            return user;
        }
    }
}
