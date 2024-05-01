using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IRouteFlightOwnerService
    {
        public Task<Airport> AddAirport(Airport airport);

        public Task<Airport> GetAirportName(int airportId);
        public Task<Models.Route> AddRoute(Models.Route route);
        public Task<Models.Route> RemoveRoute(int sourceAirportId, int destinationAirportId);
        public Task<List<Models.Route>> GetAllRoutes();
        public Task<Models.Route> GetRouteById(int routeId);
        public Task<int> GetRouteIdByAirport(int sourceAirportId, int destinationAirportId);
        public Task<List<Airport>> GetAllAirports();
        public Task<bool> RemoveRouteById(int routeId);
    }
}
