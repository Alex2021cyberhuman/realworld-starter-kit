using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.DependencyInjection
{
    public interface IInfrastructureRegistration<out TOptions>
        where TOptions : IOptions
    {
        IServiceCollection AddServices(
            IServiceCollection services,
            Action<TOptions> action);

        Task InitializeServicesAsync(AsyncServiceScope scope);
    }
}
