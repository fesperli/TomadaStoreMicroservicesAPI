using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.ConfirmSale;
using TomadaStore.PaymentAPI.Services.Interfaces;

namespace TomadaStore.PaymentAPI.Services
{
    public class PaymentService : IPaymentService
    {
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PaymentService> _logger;
        private readonly string _productURL;

        public PaymentService(IHttpClientFactory httpClientFactory, ILogger<PaymentService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _productURL = configuration["ProductApiUrl"];
        }
    
        public async Task<ProcessSaleDTO> ProcessPaymentAsync(PedidosSale pedidoEntry)
        {
            _logger.LogInformation($"[INICIO] Processando pedido. CustomerId recebido: {pedidoEntry.Id}");

            decimal totalCalculado = 0;
            var httpClient = _httpClientFactory.CreateClient();

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (pedidoEntry.Items == null || !pedidoEntry.Items.Any())
            {
                _logger.LogWarning("[AVISO] A lista de itens veio vazia!");
            }

            foreach (var item in pedidoEntry.Items)
            {
                try
                {
                    var url = $"{_productURL}{item.ProductId}";
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // 2. DEBUG: Ler como string para ver o que veio da API
                        var jsonString = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation($"[API PRODUTO] JSON Retornado: {jsonString}");

                        // 3. Deserializar usando as opções configuradas
                        var product = System.Text.Json.JsonSerializer.Deserialize<ProductResponseDTO>(jsonString, jsonOptions);

                        if (product != null)
                        {
                            decimal subtotal = product.Price * item.Quantity;
                            totalCalculado += subtotal;
                            _logger.LogInformation($"[SOMA] Prod: {product.Name} | Preço: {product.Price} * Qtd: {item.Quantity} = {subtotal}");
                        }
                        else
                        {
                            _logger.LogError("[ERRO] O produto veio nulo após deserializar.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"[ERRO API] Falha ao buscar ID {item.ProductId}. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[EXCEPTION] Erro ao processar item: {ex.Message}");
                }
            }

            string statusFinal = totalCalculado > 500 ? "Reprovado" : "Aprovado";

            _logger.LogInformation($"[FINAL] Total Calculado: {totalCalculado} | Status: {statusFinal}");

            return new ProcessSaleDTO
            {
                Id = pedidoEntry.Id, 
                CustomerId = pedidoEntry.CustomerId, 
                Items = pedidoEntry.Items,           
                TotalPrice = totalCalculado,
                Status = statusFinal
            };
        }
    }
}