using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.CustomerAPI.Service.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger,
                                 ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomerAsync([FromBody] CustomerRequestDTO customer)
        {
            try
            {
                _logger.LogInformation("CreateCustomer endpoint called.");

                await _customerService.InsertCustomerAsync(customer);

                return Ok("Customer created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating customer.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerResponseDTO>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();

                return Ok(customers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error ocurred while retrivieng all customers" + e.Message);

                return Problem(e.StackTrace);
            }



        }
    }
}
