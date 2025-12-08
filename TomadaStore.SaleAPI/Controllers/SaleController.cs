using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Controllers.v1;
using TomadaStore.SaleAPI.Services.Interfaces.v1;

namespace TomadaStore.SaleAPI.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleService _saleService;
        private readonly ConnectionFactory _connectionFactory;

        public SaleController(ILogger<SaleController> logger, ISaleService saleService, ConnectionFactory factory)
        {
            _logger = logger;
            _saleService = saleService;
            _connectionFactory = factory;
        }
        [HttpPost]

        public async Task<IActionResult> CreateSaleAsync([FromBody] SaleRequestDTO sale)
        {
            try
            {

                using var connection = await _connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "sales_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var saleMessage = System.Text.Json.JsonSerializer.Serialize(sale);
                var body = Encoding.UTF8.GetBytes(saleMessage);

                await channel.BasicPublishAsync(exchange: string.Empty ,routingKey: "sales_queue", body: body);

                _logger.LogInformation(" [x] Sent {0}", saleMessage);
                return Ok("Sale queued successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}