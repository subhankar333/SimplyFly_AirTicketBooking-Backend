using Microsoft.Extensions.Logging;
using Moq;
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
    internal class PassengerServiceTest
    {
        private PassengerService _passengerService;
        private Mock<IRepository<int,Passenger>> _mockPassengerRepository;
        private Mock<ILogger<PassengerService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockPassengerRepository = new Mock<IRepository<int, Passenger>>();
            _mockLogger = new Mock<ILogger<PassengerService>>();

            _passengerService = new PassengerService(_mockPassengerRepository.Object, _mockLogger.Object);
        }


        [Test]
        public async Task Adding_Passenger_Validate_Test()
        {
            var passenger = new Passenger
            {
                PassengerId = 1,
                Name = "Sameer Pathak",
                Age = 17,
                PassportNo = "PASS004"
            };

            _mockPassengerRepository.Setup(repo => repo.Add(passenger)).ReturnsAsync(passenger);

            //Act 
            var addedPassenger = await _passengerService.AddPassenger(passenger);

            //Assert
            Assert.AreEqual(passenger, addedPassenger);
        }

        [Test]
        public async Task RemovePassenger_ExistingPassengerId_ReturnsTrue_Test()
        {
            //arrange
            var passengerId = 1;
            var passenger = new Passenger
            {
                PassengerId = passengerId,
                Name = "Sameer Pathak",
                Age = 17,
                PassportNo = "PASS004"
            };

            _mockPassengerRepository.Setup(repo => repo.GetAsync(passengerId)).ReturnsAsync(passenger);

            //Act 
            var result = await _passengerService.RemovePassenger(passengerId);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemovePassenger_NonExistingPassengerId_ReturnsFalse()
        {
            // Arrange
            var nonExistingPassengerId = 6969;

            _mockPassengerRepository.Setup(repo => repo.GetAsync(nonExistingPassengerId)).ReturnsAsync((Passenger)null);

            // Act
            var result = await _passengerService.RemovePassenger(nonExistingPassengerId);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task GetAllPassengers_ReturnsListOfPassengers()
        {
            // Arrange
            var Passenger = new Passenger();
            Passenger.Name = "Nakash Raghuvanshi";
            Passenger.Age = 18;
            Passenger.PassportNo = "PASS009";

            var passenger2 = new Passenger 
            { 
                Name = "Harshit Roy", 
                Age = 24, 
                PassportNo = "PASS012" 
            };

            var passengers = new List<Passenger> { Passenger, passenger2 };
            _mockPassengerRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(passengers);

            // Act
            var result = await _passengerService.GetAllPassengers();

            // Assert
            Assert.AreEqual(passengers, result);
        }

        [Test]
        public async Task GetPasengerById_ExistingPassenger_Test()
        {
            //arrange
            var passengerId = 1;
            var passenger = new Passenger
            {
                PassengerId = passengerId,
                Name = "Sameer Pathak",
                Age = 17,
                PassportNo = "PASS004"
            };

            _mockPassengerRepository.Setup(repo => repo.GetAsync(passengerId)).ReturnsAsync(passenger);

            //Act 
            var result = await _passengerService.GetPassengerById(passengerId);

            //Assert
            Assert.AreEqual(result, passenger);

        }


        [Test]
        public async Task GetPasengerById_NonExistingPassenger_Test()
        {
            // Arrange
            var nonExistingPassengerId = 6969;
            _mockPassengerRepository.Setup(repo => repo.GetAsync(nonExistingPassengerId)).ReturnsAsync((Passenger)null);

            // Act
            var result = await _passengerService.GetPassengerById(nonExistingPassengerId);

            // Assert
            Assert.IsNull(result);
        }

    }
}
