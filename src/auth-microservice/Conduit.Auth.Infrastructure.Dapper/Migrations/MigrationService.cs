using System.Linq;
using System.Threading.Tasks;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Migrations
{
    public class MigrationService
    {
        private readonly IMigrationRunner _runner;
        private readonly NpgsqlConnectionOptions _options;

        public MigrationService(IOptions<NpgsqlConnectionOptions> options,
            IMigrationRunner runner)
        {
            _runner = runner;
            _options = options.Value;
        }

        public async Task InitializeAsync()
        {
            await EnsureDatabaseCreatedAsync();
            RunMigrations();
        }
        
        private async Task EnsureDatabaseCreatedAsync()
        {
            var connectionString = (string)_options.ConnectionString.Clone();

            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var database = builder.Database;
            const string databasesQuery =
                "SELECT * FROM postgres.pg_catalog.pg_database WHERE \"datname\" = @database;";
            const string createDatabaseQuery = "CREATE DATABASE @database;";
            await using var connection = new NpgsqlConnection(connectionString);
            if ((await connection.QueryAsync(databasesQuery, new { database }))
                .Any())
                return;
            await connection.ExecuteAsync(createDatabaseQuery,
                new { database });
        }

        private void RunMigrations()
        {
            _runner.MigrateUp();
        }
    }
}