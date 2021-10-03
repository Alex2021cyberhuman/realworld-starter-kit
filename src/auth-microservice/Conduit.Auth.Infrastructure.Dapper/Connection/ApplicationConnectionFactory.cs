using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public class ApplicationConnectionFactory : IApplicationConnectionFactory
    {
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection();
            await connection.OpenAsync();
            return connection;
        }
    }
}