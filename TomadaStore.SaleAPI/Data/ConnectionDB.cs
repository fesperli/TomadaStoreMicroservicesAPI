using Microsoft.Extensions.Options;
using MongoDB.Driver;

using TomadaStore.Models.Models;

namespace TomadaStore.SalesAPI.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<Sale> mongoCollection;
        public ConnectionDB(IOptions<MongoDBSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DataBaseName);
            mongoCollection = database.GetCollection<Sale>(mongoDbSettings.Value.CollectionName);
        }

        public IMongoCollection<Sale> GetMongoCollection()
        {
            return mongoCollection;
        }
    }

}