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

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null) return null;

            return new CustomerResponseDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                Email = customer.Email
            };
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
