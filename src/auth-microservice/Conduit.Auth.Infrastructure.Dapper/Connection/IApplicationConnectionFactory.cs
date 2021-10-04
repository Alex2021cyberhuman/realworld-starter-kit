using System.Threading.Tasks;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public interface IApplicationConnectionFactory
    {
        Task<NpgsqlConnection> CreateConnectionAsync();
    }
}