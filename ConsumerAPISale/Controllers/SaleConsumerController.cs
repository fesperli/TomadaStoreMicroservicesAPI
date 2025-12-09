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
        private readonly ConnectionFactory _factory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SaleConsumerController> _logger;

        public SaleConsumerController(ISaleRepository repository, IConfiguration configuration, ConnectionFactory connectionFactory, IHttpClientFactory httpClientFactory, ILogger<SaleConsumerController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _factory = connectionFactory;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost("savequeue")]
        public async Task<IActionResult> SaveSaleFromQueueAsync()
        {
            var optionsCase = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
                
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync("save_queue", false, false, false, null);

                var data = await channel.BasicGetAsync("save_queue", autoAck: false);

                if (data == null) return Ok("empty queue");

                var message = Encoding.UTF8.GetString(data.Body.ToArray());

                ProcessSaleDTO? saleQueue = JsonSerializer.Deserialize<ProcessSaleDTO>(message, optionsCase);

                if (saleQueue != null)
                {
                    var clientCustomer = _httpClientFactory.CreateClient("CustomerAPI");
                    Customer? customer = null;


                }

                ProcessSaleDTO? saleDTO = null;
                try
                {
                    saleDTO = JsonSerializer.Deserialize<ProcessSaleDTO>(message, optionsCase);
                }
                catch (Exception ex)
                {
                    _logger.LogError("JSON inválido recebido. Descartando mensagem.");
                    await channel.BasicNackAsync(data.DeliveryTag, false, false);
                    return BadRequest("JSON Inválido");
                }
                if (saleDTO != null)
                {
                    var clientCustomer = _httpClientFactory.CreateClient("CustomerAPI");
                    Customer? customer = null;
                    try
                    {
                        customer = await clientCustomer.GetFromJsonAsync<Customer>(saleDTO.CustomerId.ToString());
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogError($"{ex.Message}");
                        await channel.BasicNackAsync(data.DeliveryTag, false, false);
                        return NotFound($"{saleDTO.CustomerId}");
                    }
                    var clientProduct = _httpClientFactory.CreateClient("ProductAPI");
                    var productsList = new List<Product>();

                    foreach (var itemDTO in saleDTO.Items)
                    {
                        try
                        {
                            var product = await clientProduct.GetFromJsonAsync<Product>(itemDTO.ProductId);
                            if (product != null) productsList.Add(product);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"{itemDTO.ProductId}, {ex.Message}");
                        }
                    }
                    var novaVenda = new Sale(
                        customer!,
                        productsList,
                        saleDTO.TotalPrice,
                        status: saleDTO.Status!
                    );

                    await _repository.CreateSaleAsync(novaVenda);

                    await channel.BasicAckAsync(data.DeliveryTag, multiple: false);

                    return Ok(new { Msg = "Salvo com modelo antigo!", Id = novaVenda.Id });
                }
                return BadRequest("erro");
            } catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }

        }
           
    }
}



