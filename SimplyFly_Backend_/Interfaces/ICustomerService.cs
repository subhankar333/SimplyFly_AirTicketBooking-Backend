using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface ICustomerService
    {
        public Task<Customer> AddCustomer(Customer customer);
        public Task<bool> RemoveCustomer(int customerId);
        public Task<Customer> GetCustomerById(int customerId);
        public Task<List<Customer>> GetAllCustomers();
        public Task<Customer> GetCustomerByUsername(string username);
        public Task<Customer> UpdatedCustomerEmail(int customerId, string email);
        public Task<Customer> UpdateCustomer(UpdateCustomerDTO customer);

    }
}
