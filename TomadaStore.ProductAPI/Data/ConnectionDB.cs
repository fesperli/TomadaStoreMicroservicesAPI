using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Data;


namespace TomadaStore.CustomerAPI.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection <Product> mongoCollection; 
        private readonly string _connectionString;
        public ConnectionDB(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<Product>(mongoDbSettings.Value.CollectionName);
        }
        
        public IMongoCollection<Product> GetMongoCollection()
        {
            return mongoCollection;
        }
    }
}
