using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Service.Interfaces
{
    public interface ICustomerService
    {
        Task InsertCustomerAsync(CustomerRequestDTO customer);
        Task<List<CustomerResponseDTO>> GetAllCustomersAsync();
        Task<CustomerResponseDTO> GetCustomerByIdAsync(int id);
    }
}
