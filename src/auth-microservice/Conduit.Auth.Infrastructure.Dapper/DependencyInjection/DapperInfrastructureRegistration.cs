using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;
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
    public class DapperInfrastructureRegistration
        : IInfrastructureRegistration<DapperOptions>
    {
        #region IInfrastructureRegistration<DapperOptions> Members

        public IServiceCollection AddServices(
            IServiceCollection services,
            Action<DapperOptions> action)
        {
            var options = GetOptions(action);
            services.Configure(action)
                .AddScoped<IApplicationConnectionProvider,
                    NpgsqlConnectionProvider>()
                .AddScoped<IUsersWriteRepository, UsersWriteRepository>()
                .AddScoped<IUsersFindByEmailRepository, UsersFindByEmailRepository>()
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb.AddPostgres()
                        .WithGlobalConnectionString(
                            options.ConnectionOptions.ConnectionString)
                        .ScanIn(GetType().Assembly)
                        .For.Migrations())
                .AddTransient<MigrationService>();
            if (!CheckRepositoriesFromDomain())
                throw new InvalidOperationException(
                    "Not all repositories have been registered");

            return services;
        }

        public async Task InitializeServicesAsync(AsyncServiceScope scope)
        {
            var migrations = scope.ServiceProvider
                .GetRequiredService<MigrationService>();
            await migrations.InitializeAsync();
        }

        #endregion

        private static DapperOptions GetOptions(Action<DapperOptions> action)
        {
            var options = new DapperOptions();
            action(options);
            return options;
        }

        private static bool CheckRepositoriesFromDomain()
        {
            var repositoryInterfacesFromDomain = typeof(IRepository)
                .Assembly
                .GetTypes()
                .Where(type => type.IsInterface)
                .Where(type => type.IsAssignableFrom(typeof(IRepository)))
                .ToHashSet();
            var repositoryClassesFromThisAssembly = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.IsAssignableTo(typeof(IRepository)))
                .Where(repositoryInterfacesFromDomain.Contains);
            return repositoryInterfacesFromDomain.Count ==
                   repositoryClassesFromThisAssembly.Count();
        }
    }
}
