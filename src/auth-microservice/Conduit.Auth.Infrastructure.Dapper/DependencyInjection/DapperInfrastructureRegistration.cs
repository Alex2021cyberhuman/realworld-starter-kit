using System;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
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
        DapperInfrastructureRegistration :
            IInfrastructureRegistration<DapperOptions>
    {
        private readonly IConfiguration _configuration;

        public DapperInfrastructureRegistration(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceCollection AddServices(IServiceCollection services,
            Action<DapperOptions> action)
        {
            var options = GetOptions(action);
            return services
                .Configure(action)
                .AddScoped<IApplicationConnectionProvider,
                    NpgsqlConnectionProvider>()
                .AddScoped<IUsersWriteRepository, UsersWriteRepository>()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(options.ConnectionOptions
                        .ConnectionString)
                    .ScanIn(GetType().Assembly).For.Migrations())
                .AddTransient<MigrationService>();
        }

        public async Task InitializeServicesAsync(AsyncServiceScope scope)
        {
            var migrations =
                scope.ServiceProvider.GetRequiredService<MigrationService>();
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