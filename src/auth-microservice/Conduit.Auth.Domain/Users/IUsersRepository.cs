using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Auth.Domain.Users
{
    public interface IUsersRepository
    {
        Task<User> CreateAsync(User user,
            CancellationToken cancellationToken = default);

        Task<User> UpdateAsync(User user,
            CancellationToken cancellationToken = default);
    }
}