using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<int, Customer> _customerRepository;
        private readonly IRepository<string, User> _userRepository;
        private ILogger<CustomerService> _logger;

        public CustomerService(IRepository<int, Customer> customerRepository, IRepository<string, User> userRepository, ILogger<CustomerService> logger)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;

        }
        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customerRepository.Add(customer);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAsync();
            return customers;
        }

        public async Task<Customer> GetCustomerById(int customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);
            if(customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<Customer> GetCustomerByUsername(string username)
        {
            var customers = await _customerRepository.GetAsync();
            var customer = customers.FirstOrDefault(e => e.Username == username);

            if (customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<bool> RemoveCustomer(int customerId)
        {
            var customer = await _customerRepository.Delete(customerId);
            if (customer != null)
            {
                var user = await _userRepository.Delete(customer.Username);
                if (user != null)
                {
                    _logger.LogInformation("Customer removed with id " + customerId);
                    return true;
                }

                return true;
            }

            return false;
        }

        public async Task<Customer> UpdateCustomer(UpdateCustomerDTO customer)
        {
            var _customer = await _customerRepository.GetAsync(customer.CustomerId);
            if(_customer != null)
            {
                _customer.Name = customer.Name;
                _customer.Email = customer.Email;
                _customer.Phone = customer.Phone;
                _customer = await _customerRepository.Update(_customer);
                return _customer;
            }

            throw new NoSuchCustomerException();
        }

        public async Task<Customer> UpdatedCustomerEmail(int customerId, string email)
        {
            var customer = await _customerRepository.GetAsync(customerId);
            if(customer != null)
            {
                customer.Email = email;
                customer = await _customerRepository.Update(customer);
                return customer;
            }

            return null;
        }
    }
}
