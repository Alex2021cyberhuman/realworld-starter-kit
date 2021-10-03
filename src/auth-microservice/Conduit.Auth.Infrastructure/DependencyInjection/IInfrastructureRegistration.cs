using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.DependencyInjection
{
    public interface IInfrastructureRegistration
    {
        IServiceCollection AddServices(IServiceCollection services);

        Task InitializeServicesAsync(AsyncServiceScope scope);
    }
}