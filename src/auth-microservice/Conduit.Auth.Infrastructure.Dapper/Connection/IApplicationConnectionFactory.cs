using System.Data;
using System.Threading.Tasks;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public interface IApplicationConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}