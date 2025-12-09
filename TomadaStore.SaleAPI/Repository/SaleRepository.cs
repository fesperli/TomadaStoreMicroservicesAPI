using MongoDB.Bson;
using MongoDB.Driver;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;

        private readonly IMongoCollection<Sale> _mongoCollection;


        public SaleRepository(
            ILogger<SaleRepository> logger,
            IConfiguration configuration)
        {
            _logger = logger;

            var connectionString = configuration["MongoDBSettings:ConnectionURI"];
            var databaseName = configuration["MongoDBSettings:DataBaseName"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("MongoDB:ConnectionString", "A string de conexão não foi encontrada no appsettings.json!");
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _mongoCollection = database.GetCollection<Sale>("sales");
        }



        public async Task CreateSaleAsync(Sale sale)
        {
            await _mongoCollection.InsertOneAsync(sale);
        }
      public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _mongoCollection.Find(s => true).ToListAsync();
        }
    }
}