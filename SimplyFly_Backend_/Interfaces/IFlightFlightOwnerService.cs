using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IFlightFlightOwnerService
    {
        public Task<Flight> AddFlight(Flight flight);
        public Task<Flight> RemoveFlight(string flightNumber);
        public Task<List<Flight>> GetAllFlights();
        public Task<List<Flight>> GetAllFlightsById(int flightOwnerId);
        public Task<bool> UpdateFlightStatus(string flightNumber, int status);
        public Task<Flight> GetFlightbyId(string id);
        public Task<Flight> UpdateFlight(string flightNumber, string airline);
        public Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats);
    }
}
