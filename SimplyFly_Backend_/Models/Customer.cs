using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Customer:IEquatable<Customer>
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Username { get; set; }
        [ForeignKey("Username")]
        public User? User { get; set; }

        public Customer()
        {
            
        }
        public Customer(string name, string email, string? phone, string? gender, string password)
        {


            Name = name;
            Email = email;
            Phone = phone;
            Gender = gender;

        }

        public Customer(int customerId, string name, string email, string? phone, string? gender, string password)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
            Gender = gender;

        }

        public bool Equals(Customer? other)
        {
            var customer = other ?? new Customer();
            return this.CustomerId.Equals(customer.CustomerId) && this.Email.Equals(customer.Email);
        }
    }
}
