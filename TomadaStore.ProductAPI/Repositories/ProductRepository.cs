using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Repositories.Interfaces;

namespace TomadaStore.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMongoCollection<Product> _mongoCollection;
        private readonly ConnectionDB _connection;

        public ProductRepository(ILogger<ProductRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection;
            _mongoCollection = _connection.GetMongoCollection();
        }

        public async Task CreateProductAsync(ProductRequestDTO productDTO)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(
                 new Product
                (
                 productDTO.Name,
                 productDTO.Description,
                 productDTO.Price,
                 new Category
                 (
                     productDTO.Category.Name,
                     productDTO.Category.Description
                     )
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                throw;
            }
        }

        public Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
