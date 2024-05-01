using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class RouteService : IRouteFlightOwnerService
    {

        private readonly IRepository<int,Models.Route> _routeRepository;
        private readonly IRepository<int,Airport> _airportRepository;
        private readonly ILogger<RouteService> _logger;

        public RouteService(IRepository<int, Models.Route> routeRepository, IRepository<int, Airport> airportRepository, ILogger<RouteService> logger)
        {
            _routeRepository = routeRepository;
            _airportRepository = airportRepository;
            _logger = logger;
        }


        public async Task<Airport> AddAirport(Airport airport)
        {
            var airports = await _airportRepository.GetAsync();
            var existingAirport = airports.FirstOrDefault(a => a.City == airport.City && a.Name == airport.Name);

            if(existingAirport == null)
            {
                await _airportRepository.Add(airport);
                return airport;
            }

            throw new AirportAlreadyExistsException();
        }

        public async Task<Models.Route> AddRoute(Models.Route route)
        {
            var existingRoutes = await GetAllRoutes();
            var _route = existingRoutes.FirstOrDefault(r => r.SourceAirportId == route.SourceAirportId && r.DestinationAirportId == route.DestinationAirportId);

            if(_route == null)
            {
                
                route.Status = 1;
                route = await _routeRepository.Add(route);
                return route;
            }

            throw new RouteAlreadyExistsException();
        }

        public async Task<Airport> GetAirportName(int airportId)
        {
            var airport = await _airportRepository.GetAsync(airportId);
            if(airport != null)
            {
                return airport;
            }

            throw new NoSuchAirportException();
        }

        public async Task<List<Airport>> GetAllAirports()
        {
            var airports = await _airportRepository.GetAsync();
            return airports;
        }

        public async Task<List<Models.Route>> GetAllRoutes()
        {
            var routes = await _routeRepository.GetAsync();
            return routes;
        }

        public async Task<Models.Route> GetRouteById(int routeId)
        {
            var route = await _routeRepository.GetAsync(routeId);

            if (route != null)
            {
                return route;
            }

            throw new NoSuchRouteException();
        }

        public async Task<int> GetRouteIdByAirport(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(r => r.SourceAirportId == sourceAirportId && r.DestinationAirportId == destinationAirportId);

            if(route != null)
            {
                return (int)route.RouteId;
            }

            throw new NoSuchRouteException();
        }

        public async Task<Models.Route> RemoveRoute(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(e => e.SourceAirportId == sourceAirportId && e.DestinationAirportId == destinationAirportId);

            if( route != null )
            {
                route.Status = 0;
                await _routeRepository.Update(route);
                return route;
            }

            throw new NoSuchRouteException();
        }

        public async Task<bool> RemoveRouteById(int routeId)
        {
            var route = await _routeRepository.Delete(routeId);

            if (route != null)
                return true;

            return false;
        }
    }
}
