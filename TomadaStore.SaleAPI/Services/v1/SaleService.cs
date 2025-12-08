using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Services.Interfaces.v1;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Services.v1
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        private readonly ILogger<SaleService> _logger;

        private readonly HttpClient _httpClientProduct;
        private readonly HttpClient _httpClientCustomer;

        public SaleService(
        ISaleRepository saleRepository,
        ILogger<SaleService> logger,
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

            await _saleRepository.CreateSaleAsync(customer, items);
        }
    }
}