using TomadaStore.Models.DTOs.ConfirmSale;

namespace TomadaStore.PaymentAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ProcessSaleDTO> ProcessPaymentAsync(PedidosSale pedidoEntry);
    }
}