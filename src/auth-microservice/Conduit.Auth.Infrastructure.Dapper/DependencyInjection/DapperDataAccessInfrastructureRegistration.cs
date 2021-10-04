using System;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.Dapper.Migrations;
using Conduit.Auth.Infrastructure.Dapper.Users;
using Conduit.Auth.Infrastructure.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection
{
    public class
        DapperDataAccessInfrastructureRegistration :
            IDataAccessInfrastructureRegistration<DapperOptions>
    {
        private readonly IConfiguration _configuration;

        public DapperDataAccessInfrastructureRegistration(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection AddServices(IServiceCollection services,
            Action<DapperOptions> action)
        {
            var options = GetOptions(action);
            return services
                .Configure<NpgsqlConnectionOptions>(co => GetOptions(d =>
                {
                    d.ConnectionOptions = co;
                    action(d);
                }))
                .AddSingleton<IApplicationConnectionFactory,
                    NpgsqlConnectionFactory>()
                .AddSingleton<IUsersRepository, UsersRepository>()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(options.ConnectionOptions
                        .ConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(GetType().Assembly).For.Migrations())
                .AddTransient<DataMigrations>();
        }

        public async Task InitializeServicesAsync(AsyncServiceScope scope)
        {
            var migrations =
                scope.ServiceProvider.GetRequiredService<DataMigrations>();
            await migrations.InitializeAsync();
        }

        private static DapperOptions GetOptions(Action<DapperOptions> action)
        {
            var options = new DapperOptions();
            action(options);
            return options;
        }
    }
}