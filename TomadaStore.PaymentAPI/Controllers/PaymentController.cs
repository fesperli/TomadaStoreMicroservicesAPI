using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.ConfirmSale;
using TomadaStore.PaymentAPI.Services.Interfaces;

namespace TomadaStore.PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessarProximoDaFila()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            try
            {
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync("sales_queue", false, false, false, null);
                await channel.QueueDeclareAsync("save_queue", false, false, false, null);

                var data = await channel.BasicGetAsync("sales_queue", autoAck: false);

                if (data == null)
                {
                    return Ok("Fila vazia.");
                }

                var message = Encoding.UTF8.GetString(data.Body.ToArray());

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var pedido = JsonSerializer.Deserialize<PedidosSale>(message, options);

                if (pedido != null)
                {
                    var resultado = await _paymentService.ProcessPaymentAsync(pedido);
                    var jsonResposta = JsonSerializer.Serialize(resultado);

                    await channel.BasicPublishAsync("", "save_queue", Encoding.UTF8.GetBytes(jsonResposta));

                    await channel.BasicAckAsync(data.DeliveryTag, multiple: false);

                    return Ok(new
                    {
                        Mensagem = "Venda processada e enviada para save_queue",
                        DadosProcessados = resultado // Isso vai mostrar o ID e o Preço na tela
                    });
                }
                await channel.BasicAckAsync(data.DeliveryTag, multiple: false);
                return BadRequest("Erro ao ler JSON: Formato inválido");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no processamento: {ex.Message}");
            }
        }
    }
}

