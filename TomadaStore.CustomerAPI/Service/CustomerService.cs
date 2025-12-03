using TomadaStore.CustomerAPI.Repository;
using TomadaStore.CustomerAPI.Repository.Interfaces;
using TomadaStore.CustomerAPI.Service.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ILogger<CustomerService> logger,
                               ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomersAsync()
        {
            try
            {
               return await _customerRepository.GetAllCustomersAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                await _customerRepository.InsertCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
