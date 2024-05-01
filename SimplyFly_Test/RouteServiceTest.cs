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
    internal class RouteServiceTest
    {
        SimplyFlyDbContext context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);
        }

        [Test]
        [Order(1)]
        public async Task AddRouteTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            Airport sourceAirport = new Airport
            {
                Name = "Maharana Pratap Airport",
                City = "Udaipur",
                State = "Rajasthan",
                Country = "India"
            };
            var adddedSourceAirport = airportRepository.Add(sourceAirport);

            Airport destinationAirport = new Airport
            {
                Name = "Rajiv Gandhi Airport",
                City = "Hyderabad",
                State = "Telangana",
                Country = "India"
            };
            var addedDestinationAirport = airportRepository.Add(destinationAirport);

            Route route = new Route
            {
                SourceAirportId = adddedSourceAirport.Id,
                DestinationAirportId = addedDestinationAirport.Id,
            };

            //Act
            var _route = await routeService.AddRoute(route);

            //Assert
            Assert.That(adddedSourceAirport.Id, Is.EqualTo(_route.SourceAirportId));

        }


        [Test]
        public async Task AddAirportTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            Airport airport = new Airport
            {
                Name = "Trivandrum International Airport",
                City = "Trivandrum",
                State = "Kerela",
                Country = "India"
            };

            //Act
            var addedAirport = await routeService.AddAirport(airport);

            //Assert
            Assert.That(addedAirport.Name, Is.EqualTo(airport.Name));
        }

        [Test]
        public async Task GetAllAirportsTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            var airports = await routeService.GetAllAirports();

            Assert.IsNotEmpty(airports);
        }


        [Test]
        [Order(2)]
        public async Task GetRouteTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            
            var routes = await routeService.GetAllRoutes();
            
            Assert.IsNotEmpty(routes);
        }


        [Test]
        public async Task GetRouteByIdTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);


            //Act
            Route route = new Route
            {
                SourceAirportId = 3,
                DestinationAirportId = 1
            };

            var addedRoute = await routeService.AddRoute(route);
            var retrievedRoute = await routeService.GetRouteById(addedRoute.RouteId);

            // Assert
            Assert.That(retrievedRoute.RouteId, Is.EqualTo(addedRoute.RouteId));

        }


        [Test]
        public async Task GetRouteIdByAirportTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);


            Airport sourceAirport = new Airport
            {
                Name = "Jabalpur Airport",
                City = "Jabalpur",
                State = "Madhya Pradesh",
                Country = "India"
            };
            var addedSourceAirport = airportRepository.Add(sourceAirport);

            Airport destinationAirport = new Airport
            {
                Name = "Agra Airport",
                City = "Agra",
                State = "Uttar Pradesh",
                Country = "India"
            };
            var addedDestinationAirport = airportRepository.Add(destinationAirport);

            Route route = new Route
            {
                SourceAirportId = addedSourceAirport.Id,
                DestinationAirportId = addedDestinationAirport.Id,
            };

            var _route = await routeService.AddRoute(route);

           
            var retrievedRoute = await routeService.GetRouteIdByAirport(addedSourceAirport.Id, addedDestinationAirport.Id);
            
            Assert.That(retrievedRoute, Is.EqualTo(_route.RouteId));
        }

        [Test]
        [Order(29)]
        public async Task RemoveRouteTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            Route route = new Route
            {
                SourceAirportId = 2,
                DestinationAirportId = 1,
            };
            var routes = await routeService.AddRoute(route);

            //Act
            var deletedRoute = await routeService.RemoveRoute(route.SourceAirportId, route.DestinationAirportId);

            //Assert
            Assert.That(deletedRoute.SourceAirportId, Is.EqualTo(route.SourceAirportId));
        }

        [Test]
        public async Task NoSuchRouteExceptionTest()
        {
            //Arrange
            var mockRouteRepoLogger = new Mock<ILogger<RouteRepository>>();
            var mockRouteServiceLogger = new Mock<ILogger<RouteService>>();
            var mockAirportRepoLogger = new Mock<ILogger<AirportRepository>>();

            IRepository<int, Route> routeRepository = new RouteRepository(context, mockRouteRepoLogger.Object);
            IRepository<int, Airport> airportRepository = new AirportRepository(context, mockAirportRepoLogger.Object);
            IRouteFlightOwnerService routeService = new RouteService(routeRepository, airportRepository, mockRouteServiceLogger.Object);

            Assert.ThrowsAsync<NoSuchRouteException>(async () => await routeService.GetRouteById(6969));
        }
    }
}
