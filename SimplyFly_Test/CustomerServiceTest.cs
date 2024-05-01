using Castle.Core.Logging;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.Context;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class CustomerServiceTest
    {
        private CustomerService _customerService;
        private Mock<IRepository<int, Customer>> _mockCustomerRepository;
        private Mock<IRepository<string, User>> _mockUserRepository;
        private Mock<ILogger<CustomerService>> _mockLogger;
        SimplyFlyDbContext context;

        [SetUp]
        public void SetUp()
        {
            _mockCustomerRepository = new Mock<IRepository<int, Customer>>();
            _mockUserRepository = new Mock<IRepository<string, User>>();
            _mockLogger = new Mock<ILogger<CustomerService>>(); 

            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);
            _customerService = new CustomerService(_mockCustomerRepository.Object, _mockUserRepository.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddCustomer_Valid_ReturnsAddedCustomerTest()
        {
            var customer = new Customer()
            {
                CustomerId = 12,
                Name = "Dhruv Ghosh"
            };

            _mockCustomerRepository.Setup(repo => repo.Add(customer)).ReturnsAsync(customer);
            var addedCustomer = await _customerService.AddCustomer(customer);

            Assert.AreEqual(customer, addedCustomer);
        }


        [Test]
        public async Task GetAllCustomersTest()
        {
           
            var mockCustomerRepositoryLogger = new Mock<ILogger<CustomerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerService>>();

            IRepository<int, Customer> customerRepository = new CustomerRepository(context, mockCustomerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);

            ICustomerService customerService = new CustomerService(customerRepository, userRepository, mockCustomerServiceLogger.Object);

            Customer _customer = new Customer
            {
                Name = "CustomerTest",
                Email = "customertest@example.com",
                Phone = "9876789676",
                Username = "_customer98"
            };

            await customerRepository.Add(_customer);

            var customers = await customerService.GetAllCustomers();

            Assert.That(customers.Count, Is.EqualTo(1));
        }


        [Test]
        public async Task RemoveCustomer_ExistingCustomerId_ReturnsTrueTest()
        {
            var customerId = 12;
            var customer = new Customer
            {
                CustomerId = customerId,
                Name = "Dhruv Ghosh",
                Username = "dhruv_g" 
            };

            _mockCustomerRepository.Setup(repo => repo.Delete(customerId)).ReturnsAsync(customer);
            var result = await _customerService.RemoveCustomer(customerId);

            Assert.IsTrue(result);
        }


        [Test]
        public async Task RemoveCustomer_NonExistingCustomerId_ReturnsFalseTest()
        {
           
            var nonExistingCustomerId = 6969;
            _mockCustomerRepository.Setup(repo => repo.Delete(nonExistingCustomerId)).ReturnsAsync((Customer)(null));

            var result = await _customerService.RemoveCustomer(nonExistingCustomerId);
            Assert.IsFalse(result);
        }


        [Test]
        public async Task UpdateCustomerEmail_ExistingCustomerId_ReturnsUpdatedCustomer()
        {
            var customerId = 1;
            var updatedEmail = "dghosh@gmail.com";
            var customer = new Customer 
            { 
                CustomerId = customerId, 
                Name = "Dhruv Ghosh", 
                Email = "dhruvg@example.com" };

            _mockCustomerRepository.Setup(repo => repo.GetAsync(customerId)).ReturnsAsync(customer);
            _mockCustomerRepository.Setup(repo => repo.Update(customer)).ReturnsAsync(customer);

           
            var updatedCustomer = await _customerService.UpdatedCustomerEmail(customerId, updatedEmail);
            Assert.AreEqual(updatedEmail, updatedCustomer.Email);
        }


        [Test]
        public async Task UpdateCustomerEmail_NonExistingCustomerId_ReturnsNull()
        {
            
            var nonExistingCustomerId = 6969;
            _mockCustomerRepository.Setup(repo => repo.GetAsync(nonExistingCustomerId)).ReturnsAsync((Customer)(null));

            var updatedCustomer = await _customerService.UpdatedCustomerEmail(nonExistingCustomerId, "test@example.com");
            Assert.IsNull(updatedCustomer);
        }


        [Test]
        public async Task GetByIdCustomers_ExistingCustomerId_ReturnsCustomerTest()
        {
            
            var customerId = 1;
            var customer = new Customer { CustomerId = customerId, Name = "Dhruv Ghosh" };

            _mockCustomerRepository.Setup(repo => repo.GetAsync(customerId)).ReturnsAsync(customer);
            var retrievedCustomer = await _customerService.GetCustomerById(customerId);

            Assert.AreEqual(customer, retrievedCustomer);
        }


        [Test]
        public async Task GetCustomersByUsernameTest()
        {
            var mockCustomerRepositoryLogger = new Mock<ILogger<CustomerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerService>>();

            IRepository<int, Customer> customerRepository = new CustomerRepository(context, mockCustomerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);

            ICustomerService customerService = new CustomerService(customerRepository, userRepository, mockCustomerServiceLogger.Object);

            Customer customer = new Customer
            {
                Name = "Raghav Raman",
                Email = "raghav@gmail.com",
                Phone = "9987658978",
                Username = "rg_raman"
            };

            await customerRepository.Add(customer);

            var retrievedCustomer = await customerService.GetCustomerByUsername("rg_raman");
            Assert.That(retrievedCustomer, Is.EqualTo(customer));
        }


        [Test]
        public async Task UpdateCustomerTest()
        {
            var mockCustomerRepositoryLogger = new Mock<ILogger<CustomerRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockCustomerServiceLogger = new Mock<ILogger<CustomerService>>();

            IRepository<int, Customer> customerRepository = new CustomerRepository(context, mockCustomerRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);

            ICustomerService customerService = new CustomerService(customerRepository, userRepository, mockCustomerServiceLogger.Object);

            Customer customer = new Customer
            {
                Name = "Pranab Rao",
                Email = "prao@gmail.com",
                Phone = "8768964387",
                Username = "pr_rao"
            };

            await customerRepository.Add(customer);

            var updatedCustomerDTO = new UpdateCustomerDTO
            {
                CustomerId = customer.CustomerId,
                Name = "Pranab R",
                Email = "prananbrao@gmail.com",
                Phone = "8768964388"
            };

            // Act
            var updatedCustomer = await customerService.UpdateCustomer(updatedCustomerDTO);

            // Assert
            Assert.That(updatedCustomer.Name, Is.EqualTo(updatedCustomerDTO.Name));

        }

    }
}
