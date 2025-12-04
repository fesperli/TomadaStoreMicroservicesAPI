namespace TomadaStore.Models.Models
{
    public class Customer
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public bool Status { get; private set; }
        public Customer(string firstName, string lastName, string email, bool status)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Status = status;
        }

        public Customer(string firstName, string lastName, string email, bool status, string? phoneNumber)
                       : this(firstName, lastName, email, status)
        {
            PhoneNumber = phoneNumber;
        }

    }
}
