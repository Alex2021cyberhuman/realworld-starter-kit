using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Conduit.Auth.Infrastructure.Dapper.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IApplicationConnectionFactory _factory;
        private readonly Compiler _compiler;

        public UsersRepository(IApplicationConnectionFactory factory, Compiler compiler)
        {
            _factory = factory;
            _compiler = compiler;
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            using var connection = await _factory.CreateConnectionAsync();
            var insertedUser = await new XQuery(connection, _compiler)
                .InsertGetIdAsync<User>(user.AsColumns(),
                    cancellationToken: cancellationToken);
            return insertedUser;
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            using var connection = await _factory.CreateConnectionAsync();
            var insertedRows= await new XQuery(connection, _compiler)
                .Where(UsersColumns.Id, user.Id)
                .UpdateAsync(user.AsColumns(), cancellationToken: cancellationToken);
            if (insertedRows != 1)
                throw new InvalidProgramException();
            return user;
        }
    }
}