using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Mappers
{
    [ExcludeFromCodeCoverage]
    public class RegisterToCustomer
    {
        Customer customer;
        public RegisterToCustomer(RegisterCustomerUserDTO register)
        {
            customer = new Customer();
            customer.Name = register.Name;
            customer.Email = register.Email;
            customer.Phone = register.Phone;
            customer.Gender = register.Gender;
            customer.Username = register.Username;
        }

        public Customer GetCustomer()
        {
            return customer;
        }
    }
}
