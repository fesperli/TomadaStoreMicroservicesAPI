using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace TomadaStore.Models.Models
{
    public class Customer
    {
        [JsonPropertyName("id")]
        public int Id { get; private set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; private set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; private set; }

        [JsonPropertyName("email")]
        public string Email { get; private set; }

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; private set; }

        public Customer() { }

        [BsonConstructor]
        public Customer(
            int id,
            string firstName,
            string lastName,
            string email,
            string? phoneNumber
        )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}