using RabbitMQ.Client;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Services.Interfaces.v1;
using TomadaStore.SaleAPI.Services.Interfaces.v2;
using TomadaStore.SaleAPI.Services.v2;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Services.v2
{
    public class SaleServiceV2 : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<SaleServiceV2> _logger;
        private readonly HttpClient _httpClientProduct;
        private readonly HttpClient _httpClientCustomer;

        public SaleServiceV2(
        ISaleRepository saleRepository,
        ILogger<SaleServiceV2> logger,
        IHttpClientFactory factory)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _httpClientCustomer = factory.CreateClient("CustomerAPI");
            _httpClientProduct = factory.CreateClient("ProductAPI");
        }
        public async Task CreateSaleAsync(int idCustomer, List<SaleItemDTO> itemsDTO)
        {
            var customer = await _httpClientCustomer.GetFromJsonAsync<CustomerResponseDTO>(idCustomer.ToString());
            var items = new List<(ProductResponseDTO product, int quantity)>();
            foreach (var item in itemsDTO)
            {
                var product = await _httpClientProduct.GetFromJsonAsync<ProductResponseDTO>(item.ProductId);
                items.Add((product, item.Quantity));
            }
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sales_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = JsonSerializer.Serialize(new { Customer = customer, Items = itemsDTO });
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                         routingKey: "sales_queue",
                                         body: body);
        }

    }
}
