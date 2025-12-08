using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.SaleAPI.Services.Interfaces.v2
{
    public interface ISaleServiceV2
    {
        Task CreateSaleAsync(int idCustomer, List<SaleItemDTO> itemsDTO);
    }
}
