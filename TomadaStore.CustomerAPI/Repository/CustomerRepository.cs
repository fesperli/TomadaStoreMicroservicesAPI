using Dapper;
using Microsoft.Data.SqlClient;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.CustomerAPI.Repository.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly SqlConnection _connection;

        public CustomerRepository(ILogger<CustomerRepository> logger,
                                  ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection.GetConnection();
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomersAsync()
        {
            try
            {
                var sqlSelect = @"SELECT Id, FirstName, LastName, Email, PhoneNumber
                                  FROM Customers";

                var customers = await _connection.QueryAsync<CustomerResponseDTO>(sqlSelect);

                return customers.ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"A SQL error occurred while retrieving customers: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving customers: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<CustomerResponseDTO> GetCustomerByIdAsync(int id)
        {
            try
            {
                var insertSql = @"SELECT Id, FirstName, LastName, Email, PhoneNumber
                                    FROM Customers
                                    WHERE Id = @CustomerId";
                return await _connection.QueryFirstOrDefaultAsync<CustomerResponseDTO>(insertSql, new { CustomerId = id });
            }
            catch (SqlException e)
            {
               _logger.LogError($"Could not find {e.Message}");
                throw new Exception(e.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not find {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                var insertSql = @"INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber)  
                                  VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";

                await _connection.ExecuteAsync(insertSql, new
                {
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.PhoneNumber          
                });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"A SQL error occurred while inserting a customer: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while inserting a customer: {ex.Message}");
                throw new Exception(ex.StackTrace);
            }
        }
    }
}


