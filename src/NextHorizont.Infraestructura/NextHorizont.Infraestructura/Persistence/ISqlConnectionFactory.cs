using System.Data;
using System.Threading.Tasks;

namespace NextHorizont.Infraestructura.Persistence;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
