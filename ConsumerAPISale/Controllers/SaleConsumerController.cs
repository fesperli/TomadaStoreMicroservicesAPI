using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.ConfirmSale;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace ConsumerAPISale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleConsumerController : ControllerBase
    {
        private readonly ISaleRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;

        public SaleConsumerController(ISaleRepository repository, IConfiguration configuration, ConnectionFactory connectionFactory, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _configuration = configuration;
            _factory = connectionFactory;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("savequeue")]
        public async Task<IActionResult> SaveSaleFromQueueAsync()
        {

            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync("save_queue", false, false, false, null);

                var data = await channel.BasicGetAsync("save_queue", autoAck: false);

                if (data == null) return Ok("empty queue");

                var message = Encoding.UTF8.GetString(data.Body.ToArray());
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var vendaDto = JsonSerializer.Deserialize<ProcessSaleDTO>(message, options);

                if (vendaDto != null)
                {

                    var client = _httpClientFactory.CreateClient();

                    var customerUrl = $"https://localhost:5001/api/Customer/{vendaDto.CustomerId}";
                    var customer = await client.GetFromJsonAsync<Customer>(customerUrl);

                    var productsList = new List<Product>();

                    foreach (var itemDto in vendaDto.Items)
                    {
                        var productUrl = $"https://localhost:6001/api/v1/Product/{itemDto.ProductId}";
                        var product = await client.GetFromJsonAsync<Product>(productUrl);

                        if (product != null)
                        {
                            productsList.Add(product);
                        }
                    }

                    var novaVenda = new Sale(
                        customer,
                        productsList,
                        vendaDto.TotalPrice,
                        status: vendaDto.Status!
                    );

                    await _repository.CreateSaleAsync(novaVenda);

                    await channel.BasicAckAsync(data.DeliveryTag, multiple: false);

                    return Ok(new { Msg = "Salvo com modelo antigo!", Id = novaVenda.Id });
                }

                await channel.BasicAckAsync(data.DeliveryTag, multiple: false);
                return BadRequest("JSON inválido.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}

