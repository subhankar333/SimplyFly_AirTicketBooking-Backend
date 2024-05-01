using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CustomerRepository:IRepository<int, Customer>
    {
        private readonly SimplyFlyDbContext _context;
        ILogger<CustomerRepository> _logger;

        public CustomerRepository(SimplyFlyDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> Add(Customer item)
        {
            _context.Customers.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"Admin added with id-{item.CustomerId}");
            return item;
        }

        public async Task<Customer> Delete(int customerId)
        {
            var customer = await GetAsync(customerId);
            if(customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
                _logger.LogInformation($"Customer deleted with id {customerId}");
            }
            throw new NoSuchCustomerException();
        }

        public async Task<Customer> GetAsync(int customerId)
        {
            var customers =  _context.Customers.ToList();
            var customer = customers.FirstOrDefault(c => c.CustomerId == customerId);

            if (customer != null)
            {
                return customer;
            }

            throw new NoSuchCustomerException();
        }

        public async Task<List<Customer>> GetAsync()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }

        public async Task<Customer> Update(Customer item)
        {
            var customer = await GetAsync(item.CustomerId);
            if (customer != null)
            {
                _context.Entry<Customer>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Customer updated with id {item.CustomerId}");
                return customer;
            }
            throw new NoSuchCustomerException();
        }
    }
}
