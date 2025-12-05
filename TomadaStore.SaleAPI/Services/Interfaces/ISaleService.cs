using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SalesAPI.Repositories.Interfaces;

namespace TomadaStore.SalesAPI.Services.Interfaces
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int idCustomer, List<SaleItemDTO> itemsDTO);
    }
}