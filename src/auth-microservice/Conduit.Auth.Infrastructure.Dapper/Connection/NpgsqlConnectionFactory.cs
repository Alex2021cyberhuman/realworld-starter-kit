using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public class NpgsqlConnectionFactory : IApplicationConnectionFactory
    {
        private readonly IOptionsMonitor<NpgsqlConnectionOptions>
            _optionsMonitor;

        public NpgsqlConnectionFactory(
            IOptionsMonitor<NpgsqlConnectionOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            var options = _optionsMonitor.CurrentValue;
            var connectionsString = _optionsMonitor.CurrentValue;
            var connection = new NpgsqlConnection();
            await connection.OpenAsync();
            return connection;
        }
    }
}