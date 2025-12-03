using System.Data;
using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    public abstract class DBConnectionFactory
    {
        public abstract IDBConnection CreateDBConnection();

        public string ConnectionString()
        {
           var dBConnection = CreateDBConnection();
           return dBConnection.ConnectionString();
        }
    }

}
