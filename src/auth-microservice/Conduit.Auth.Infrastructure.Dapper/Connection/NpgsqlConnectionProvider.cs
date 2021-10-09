using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public class NpgsqlConnectionProvider : IApplicationConnectionProvider, IDisposable
    {
        private readonly IOptionsMonitor<NpgsqlConnectionOptions>
            _optionsMonitor;

        private NpgsqlConnection? _currentScopeConnection;

        public NpgsqlConnectionProvider(
            IOptionsMonitor<NpgsqlConnectionOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            var options = _optionsMonitor.CurrentValue;
            var connectionsString = options.ConnectionString;
            if (_currentScopeConnection is not null)
                return _currentScopeConnection; 
            _currentScopeConnection = new(connectionsString);
            await _currentScopeConnection.OpenAsync();
            return _currentScopeConnection;
        }

        public void Dispose()
        {
            _currentScopeConnection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}