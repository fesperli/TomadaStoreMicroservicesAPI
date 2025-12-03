using Microsoft.Extensions.Configuration;
using System.Data;
using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    internal class SQLDBConnectionImpl : IDBConnection
    {
        private readonly string _connctionString;
        private readonly IConfiguration configuration;

        public SQLDBConnectionImpl()
        {
            _connctionString = configuration.GetConnectionString("SqlServer");
        }
        public string ConnectionString()
        {
            return _connctionString;
        }

    }
}