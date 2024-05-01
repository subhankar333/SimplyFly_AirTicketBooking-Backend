using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
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
    internal class FlightServiceTest
    {
        SimplyFlyDbContext context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);
        }

        [Order(3)]
        [Test]
        public async Task AddFlightTest()
        {
            //Arrange
            var mockFlightRepostoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepostoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);

            Flight flight = new Flight
            {
                FlightNumber = "FLI006",
                Airline = "Air King",
                TotalSeats = 100,
                FlightOwnerId = 1
            };

            //Act 
            var addedFlight = await flightOwnerService.AddFlight(flight);

            //Assert
            Assert.That(addedFlight.FlightNumber, Is.EqualTo(flight.FlightNumber));
        }

        [Order(4)]
        [Test]
        public async Task GetAllFlightTest()
        {
            //Arrange
            var mockFlightRepostoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepostoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);

            var flights = await flightOwnerService.GetAllFlights();
            
            Assert.IsNotEmpty(flights);
        }

        [Order(5)]
        [Test]
        public async Task UpdateAirlineTest()
        {
            //Arrange
            var mockFlightRepostoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepostoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);

            Flight flight = new Flight
            {
                Airline = "Air King 2.0"
            };

            var updatedFlight = await flightOwnerService.UpdateFlight("FLI006", "Air King 2.0");

            Assert.That(updatedFlight.Airline, Is.EqualTo(flight.Airline));

        }

        [Order(6)]
        [Test]
        public async Task UpdateTotalSeatsTest()
        {
            //Arrange
            var mockFlightRepositoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepositoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);

            Flight flight = new Flight
            {
                TotalSeats = 105
            };

            //Act
            var updatedFlight = await flightOwnerService.UpdateTotalSeats("FLI006", flight.TotalSeats);

            //Assert
            Assert.That(updatedFlight.TotalSeats, Is.EqualTo(flight.TotalSeats));
        }

        [Order(33)]
        [Test]
        public async Task RemoveFlightTest()
        {
            //Arrange
            var mockFlightRepositoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepositoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);

         
            var flight = await flightOwnerService.RemoveFlight("FLI006");
            
            Assert.That(flight.FlightNumber, Is.EqualTo("FLI006"));
        }

        [Test]
        public async Task NoSuchFlightExceptionTest()
        {
            //Arrange
            var mockFlightRepositoryLogger = new Mock<ILogger<FlightRepository>>();
            var mockFlightServiceLogger = new Mock<ILogger<FlightService>>();

            IRepository<string, Flight> flightRepository = new FlightRepository(context, mockFlightRepositoryLogger.Object);
            IFlightFlightOwnerService flightOwnerService = new FlightService(flightRepository, mockFlightServiceLogger.Object);
            
            Assert.ThrowsAsync<NoSuchFlightException>(async () => await flightOwnerService.GetFlightbyId("FLI6969"));
        }
    }
}
