using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Services.Interfaces.v1;

namespace TomadaStore.SaleAPI.Controllers.v2
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
                var saleSave = await _saleService.CreateSaleAsync(sale.CustomerId, sale.Items);
                using var connection = await _connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "sales_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var saleMessage = System.Text.Json.JsonSerializer.Serialize(saleSave);
                var body = Encoding.UTF8.GetBytes(saleMessage);

                await channel.BasicPublishAsync(exchange: string.Empty ,routingKey: "sales_queue", body: body);

                _logger.LogInformation(" [x] Sent {0}", saleMessage);
                return Ok(saleSave);
            }
            catch (Exception e)
            {
                
                return BadRequest($"ERRO DETALHADO: {e.ToString()}");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<SaleResponseDTO>>> GetAllSalesAsync()
        {
            try
            {
                var customers = await _saleService.GetAllSalesAsync();

                return Ok(customers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error ocurred while retrivieng all sales" + e.Message);

                return Problem(e.StackTrace);
            }
        }

    }
}