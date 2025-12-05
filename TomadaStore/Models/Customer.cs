using MongoDB.Bson.Serialization.Attributes;

namespace TomadaStore.Models.Models
{
    public class Customer
    {
        [BsonElement("id")]
        public int Id { get; private set; }

        [BsonElement("firstName")]
        public string FirstName { get; private set; }

        [BsonElement("lastName")]
        public string LastName { get; private set; }

        [BsonElement("email")]
        public string Email { get; private set; }

        [BsonElement("phoneNumber")]
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