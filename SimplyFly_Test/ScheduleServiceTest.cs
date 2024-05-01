using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SimplyFly_Project.Context;
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
    [TestFixture]
    internal class ScheduleServiceTest
    {
        SimplyFlyDbContext context;
        private ScheduleService _scheduleService;
        private Mock<IRepository<int, Schedule>> _mockScheduleRepository;
        private Mock<IRepository<string,Flight>>  _mockFlightRepository;
        private Mock<IRepository<int, Passenger>> _mockPassengerRepository;
        private Mock<IBookingService> _mockBookingService;
        private Mock<IScheduleFlightOwnerService> _mockScheduleFlightOwnerService;
        private Mock<ISeatDetailService> _mockSeatDetailService;
        private Mock<ILogger<ScheduleService>> _mockLogger;


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);

            _mockScheduleRepository = new Mock<IRepository<int, Schedule>>();
            _mockFlightRepository = new Mock<IRepository<string, Flight>>();
            _mockPassengerRepository = new Mock<IRepository<int, Passenger>>();
            _mockBookingService = new Mock<IBookingService>();
            _mockScheduleFlightOwnerService = new Mock<IScheduleFlightOwnerService>();
            _mockSeatDetailService = new Mock<ISeatDetailService>();
            _mockLogger = new Mock<ILogger<ScheduleService>>();

            _scheduleService = new ScheduleService(_mockScheduleRepository.Object, _mockBookingService.Object, _mockLogger.Object, _mockSeatDetailService.Object, _mockPassengerRepository.Object, _mockFlightRepository.Object);

        }


        [Test]
        [Order(1)]
        public async Task AddScheduleTest()
        {
            // Arrange
            Schedule schedule = new Schedule
            {
                FlightNumber = "FLI007",
                Departure = DateTime.Now.AddDays(2),
                Arrival = DateTime.Now.AddDays(3),
                RouteId = 1
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(new List<Schedule>());
            _mockScheduleRepository.Setup(repo => repo.Add(It.IsAny<Schedule>())).ReturnsAsync(schedule);

            //Act
            var addedSchedule = await _scheduleService.AddSchedule(schedule);

            //Assert
            Assert.That(schedule.ScheduleId, Is.EqualTo(addedSchedule.ScheduleId));

        }

        [Test]
        [Order(8)]
        public async Task GetAllSchedulesTest()
        {
            // Arrange
            Schedule schedule = new Schedule
            {
                FlightNumber = "FLI007",
                Departure = DateTime.Now.AddDays(2),
                Arrival = DateTime.Now.AddDays(3),
                RouteId = 1
            };

            var scheduleList = new List<Schedule>();
            scheduleList.Add(schedule);

            _mockScheduleRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(scheduleList);

            //Act 
            var schedules = await _scheduleService.GetAllSchedules();

            //Assert
            Assert.IsNotEmpty(schedules);
        }

        [Test]
        public async Task RemoveScheduleTest()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                FlightNumber = "FLI008",
                Departure = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync(schedule.ScheduleId)).ReturnsAsync(schedule);
            _mockScheduleRepository.Setup(repo => repo.Delete(schedule.ScheduleId)).ReturnsAsync(schedule);

            // Act
            var removedSchedule = await _scheduleService.RemoveSchedule(schedule);

            // Assert
            Assert.That(removedSchedule, Is.EqualTo(schedule));
        }

        [Test]
        public async Task RemoveSchedule_By_FlightNumber_Test()
        {
            //arrange
            var flightNumber = "FLI008";
            var schedule = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    FlightNumber = flightNumber,
                    Departure = DateTime.UtcNow,
                    Arrival = DateTime.UtcNow.AddHours(2),
                }
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(schedule);
            _mockScheduleRepository.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(new Schedule());

            // Act
            var removedSchedule = await _scheduleService.RemoveSchedule(flightNumber);

            // Assert
            Assert.That(removedSchedule, Is.EqualTo(schedule.Count));


        }

        [Test]
        public async Task RemoveSchedule_By_DepartureDate_Test()
        {
            var departureDate = DateTime.UtcNow;
            var airportId = 1;

            var schedulesToRemove = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    FlightNumber = "FLI008",
                    Departure = DateTime.UtcNow,
                    Arrival = DateTime.UtcNow.AddHours(2),
                    Route = new Route{SourceAirportId =  airportId},
                }
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(schedulesToRemove);
            _mockScheduleRepository.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(new Schedule());

            // Act
            var removedScheduleCount = await _scheduleService.RemoveSchedule(departureDate, airportId);

            // Assert
            Assert.That(removedScheduleCount, Is.EqualTo(schedulesToRemove.Count));

        }

        [Test]
        public async Task UpdateScheduleFlight()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                FlightNumber = "FLI008",
                Departure = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2)
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync(schedule.ScheduleId)).ReturnsAsync(schedule);
            _mockScheduleRepository.Setup(repo => repo.Update(schedule)).ReturnsAsync(schedule);

            //Act
            var updatedSchedule = await _scheduleService.UpdateScheduledFlight(schedule.ScheduleId, "FLI009");

            //Assert
            Assert.That(schedule.ScheduleId, Is.EqualTo(updatedSchedule.ScheduleId));
        }

        [Test]
        public async Task Update_Schedule_Route_Test()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                FlightNumber = "FLI008",
                Departure = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2),
                RouteId = 1
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync(schedule.ScheduleId)).ReturnsAsync(schedule);
            _mockScheduleRepository.Setup(repo => repo.Update(schedule)).ReturnsAsync(schedule);

            // Act
            var updatedSchedule = await _scheduleService.UpdateScheduledRoute(schedule.ScheduleId, 2);

            // Assert
            Assert.That(updatedSchedule.RouteId, Is.EqualTo(2));
        }


        [Test]
        public async Task UpdateScheduleTime_Test()
        {
            // Arrange
            var schedule = new Schedule
            {
                ScheduleId = 1,
                FlightNumber = "FLI008",
                Departure = DateTime.UtcNow,
                Arrival = DateTime.UtcNow.AddHours(2),
                RouteId = 1
            };

            _mockScheduleRepository.Setup(repo => repo.GetAsync(schedule.ScheduleId)).ReturnsAsync(schedule);
            _mockScheduleRepository.Setup(repo => repo.Update(schedule)).ReturnsAsync(schedule);

            DateTime departure = DateTime.Now.AddHours(2);

            //Act
            var updateSchedule = await _scheduleService.UpdateScheduledTime(schedule.ScheduleId, departure, DateTime.Now.AddHours(6));

            //Assert
            Assert.That(updateSchedule.Departure, Is.EqualTo(departure));
        }

        [Test]
        public void Calculate_TotalPrice_Test()
        {
            // Arrange
            var searchFlightDto = new SearchFlightDTO
            {
                SeatClass = "Economy",
                Adult = 2,
                Child = 1
            };
            var basePrice = 1000.0;

            // Act
            var totalPrice = _scheduleService.CalculateTotalPrice(searchFlightDto, basePrice);

            // Assert
            Assert.That(totalPrice, Is.EqualTo(4400.0));
        }

        [Test]
        public async Task GetFlightSchedules()
        {
            // Arrange
            var flightNumber = "FLI008";
            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    FlightNumber = flightNumber,
                    Route = new Route
                    {
                        SourceAirport = new Airport { Name = "Raipur Airport", City = "Raipur" },
                        DestinationAirport = new Airport { Name = "Ahmadabad Airport", City = "Ahmadabad" }
                    },
                    Departure = DateTime.Now,
                    Arrival = DateTime.Now.AddHours(2)
                }
            };

            _mockScheduleRepository.Setup(r => r.GetAsync()).ReturnsAsync(schedules);

            // Act
            var result = await _scheduleService.GetFlightSchedule(flightNumber);

            // Assert
            Assert.That(result, Is.Not.Null);
        }


        [Test]
        public async Task SearchFlights_Return_Correct_SearchedFlightResults()
        {
            // Arrange
            var searchFlight = new SearchFlightDTO
            {
                DateOfJourney = DateTime.Now.Date,
                Origin = "Kolkata",
                Destination = "Delhi",
                Adult = 1,
                Child = 1,
                SeatClass = "Economy"
            };

            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    ScheduleId = 1,
                    RouteId = 1,
                    FlightNumber = "FLI009",
                    Route = new Route
                    {
                        RouteId = 1,
                        SourceAirportId = 3,
                        SourceAirport = new Airport { City = "Kolkata" },
                        DestinationAirportId = 5,
                        DestinationAirport = new Airport { City = "Delhi" }
                    },
                    Flight = new Flight
                    {
                        FlightNumber = "FLI009",
                        Airline = "Air King",
                        BasePrice = 4000.0,
                        TotalSeats = 100
                    },
                    Departure = DateTime.Now.Date,
                    Arrival = DateTime.Now.Date.AddHours(2),

                }
            };

            //var mockScheduleRepository = new Mock<IRepository<int, Schedule>>();
            _mockScheduleRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(schedules);

            
            // Act
            var result = await _scheduleService.SearchFlights(searchFlight);

            // Assert
            Assert.IsNotEmpty(result);
            
        }


        [Test]
        public async Task NoSuchScheduleExceptionTest()
        {
            // Arrange
            int scheduleId = 6969;

            _mockScheduleRepository.Setup(repo => repo.GetAsync(scheduleId)).ThrowsAsync(new NoSuchScheduleException());

            // Act & Assert
            Assert.ThrowsAsync<NoSuchScheduleException>(async () => await _scheduleService.UpdateScheduledFlight(scheduleId, "FLI6969"));
        }

    }
}
