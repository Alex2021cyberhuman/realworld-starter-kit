using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.DependencyInjection;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection
{
    public class DapperOptions : IOptions
    {
        public NpgsqlConnectionOptions ConnectionOptions { get; set; } = new();
    }
}
