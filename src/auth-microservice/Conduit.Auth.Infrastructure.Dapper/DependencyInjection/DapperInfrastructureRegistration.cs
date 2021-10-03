using System.Threading.Tasks;
using Conduit.Auth.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection
{
    public class DapperInfrastructureRegistration : IInfrastructureRegistration
    {
        public IServiceCollection AddServices(IServiceCollection services)
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeServicesAsync(AsyncServiceScope scope)
        {
            throw new System.NotImplementedException();
        }
    }
}