using RabbitMQ.Client;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Services.Interfaces.v1;
using TomadaStore.SaleAPI.Services.v2;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Services.v2
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<SaleService> _logger;

        public SaleService(
        ISaleRepository saleRepository,
        ILogger<SaleService> logger,
        IHttpClientFactory factory)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }
        
        public async Task<SaleResponseDTO> CreateSaleAsync(int idCustomer, List<SaleItemDTO> itemsDTO)
        {

            var vendaParaFila = new
            {
                CustomerId = idCustomer,
                Items = itemsDTO,
                Status = "Pendente Processamento"
            };
            var factory = new ConnectionFactory { HostName = "localhost" };

            try
            {
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "sales_queue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                var message = JsonSerializer.Serialize(vendaParaFila);
                var body = System.Text.Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(exchange: string.Empty,
                                             routingKey: "sales_queue",
                                             body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw ex;
            }

            return new SaleResponseDTO
            {
                Id = 0, 
                CustomerId = idCustomer,
                Items = itemsDTO,
                Status = "Enviado para fila"
            };
        }
        public async Task<List<SaleResponseDTO>> GetAllSalesAsync()
        {
            try
            {
                var sales = await _saleRepository.GetAllSalesAsync();

                var saleDto = sales.Select(sale => new SaleResponseDTO
                {
                    Items = sale.Products.Select(product => new SaleItemDTO
                    {
                        ProductId = product.Id.ToString(),
                        Quantity = product.Quantity
                    }).ToList()
                }).ToList();

                return saleDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

    }
}
