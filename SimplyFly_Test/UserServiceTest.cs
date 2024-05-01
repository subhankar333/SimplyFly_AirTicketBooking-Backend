using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class UserServiceTest
    {
        private UserService _userService;
        private Mock<IRepository<string, User>> _mockUserRepository;
        private Mock<IRepository<int, Admin>> _mockAdminRepository;
        private Mock<IRepository<int, FlightOwner>> _mockFlightOwnerRepository;
        private Mock<IRepository<int, Customer>> _mockCustomerRepository;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILogger<UserService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IRepository<string, User>>();
            _mockAdminRepository = new Mock<IRepository<int, Admin>>();
            _mockFlightOwnerRepository = new Mock<IRepository<int, FlightOwner>>();
            _mockCustomerRepository = new Mock<IRepository<int, Customer>>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _userService = new UserService(_mockUserRepository.Object, _mockAdminRepository.Object, _mockCustomerRepository.Object, _mockFlightOwnerRepository.Object, _mockLogger.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task RegisterAdminTest()
        {
            // Arrange
            var admin = new RegisterAdninUserDTO
            {
                Username = "ronin_29",
                Password = "Ronin@29",
                Role = "admin",
                Name = "Ronin Paul",
                Email = "ronin@gmail.com",
                Position = "Manager",
                ContactNumber = "986523498",
                Address = "Madhya Pradesh"
            };

            var existingUsers = new List<User>
            {
                new User { Username = "prakash_10" }
            };

            _mockUserRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(existingUsers);
            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync((User user) => user);
            _mockAdminRepository.Setup(repo => repo.Add(It.IsAny<Admin>())).ReturnsAsync(new Admin());

            // Act
            var registeredAdmin = await _userService.RegisterAdmin(admin);

            // Assert
            Assert.That(registeredAdmin.Username, Is.EqualTo(admin.Username));
        }


        [Test]
        public async Task Update_Password_Test()
        {
            //arrange
            var forgotPasswordDTO = new ForgotPasswordDTO
            {
                Username = "prakash_10",
                Password = "Prakash10@"
            };

            var existingUser = new User
            {
                Username = forgotPasswordDTO.Username
            };

            _mockUserRepository.Setup(repo => repo.GetAsync(forgotPasswordDTO.Username)).ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>())).ReturnsAsync((User user) => user);

            //act
            var updatedPassword = await _userService.UpdateUserPassword(forgotPasswordDTO);

            //asert
            Assert.AreEqual(forgotPasswordDTO.Username, updatedPassword.Username);
            Assert.AreEqual(updatedPassword.Role, existingUser.Role);
        }


        [Test]
        public async Task RegisterFlightOwnerTest()
        {
            // Arrange
            var flightOwnerUser = new RegisterFlightOwnerUserDTO
            {
                Username = "karan_a",
                Password = "Karan@",
                Role = "flightowner",
            };

            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync((User user) => user);
            _mockFlightOwnerRepository.Setup(repo => repo.Add(It.IsAny<FlightOwner>())).ReturnsAsync(new FlightOwner());

            // Act
            var registeredFlightOwner = await _userService.RegisterFlightOwner(flightOwnerUser);

            // Assert
            Assert.That(registeredFlightOwner.Username, Is.EqualTo(flightOwnerUser.Username));
        }



        [Test]
        public async Task Register_Customer_Test()
        {
            // Arrange
            var customerUser = new RegisterCustomerUserDTO
            {
                Username = "rakesh_67",
                Password = "Rakesh@67",
                Role = "customer",
            };

            _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>())).ReturnsAsync((User user) => user);

            _mockCustomerRepository.Setup(repo => repo.Add(It.IsAny<Customer>())).ReturnsAsync(new Customer());

            // Act
            var result = await _userService.RegisterCustomer(customerUser);

            // Assert
            Assert.That(result.Username, Is.EqualTo(customerUser.Username));
        }

        [Test]
        public async Task Compare_Passwords_Test()
        {
            byte[] password = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            byte[] userPassword = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var IsCorrect = _userService.ComparePasswords(password, userPassword);
            Assert.AreEqual(IsCorrect, true);
        }


        [Test]
        public void Invalid_User_Exception_Test()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO
            {
                Username = "rakhim",
                Password = "Rakhim@",
                Role = "customer    "
            };

            _mockUserRepository.Setup(repo => repo.GetAsync("rakhim")).ReturnsAsync((User)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidUserException>(async () => await _userService.Login(loginUserDTO));
        }
    }
}
