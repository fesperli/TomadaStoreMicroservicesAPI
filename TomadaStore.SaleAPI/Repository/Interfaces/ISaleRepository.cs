using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;

namespace TomadaStore.SalesAPI.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        Task CreateSaleAsync(Sale sale);

        Task <List<Sale>>GetAllSalesAsync();
    }
}