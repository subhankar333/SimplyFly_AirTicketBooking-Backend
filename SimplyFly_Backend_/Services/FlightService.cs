using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class FlightService : IFlightFlightOwnerService
    {
        private readonly IRepository<string,Flight> _flightRepository;
        private readonly ILogger<FlightService> _logger;

        public FlightService(IRepository<string, Flight> flightRepository, ILogger<FlightService> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;

        }


        public async Task<Flight> AddFlight(Flight flight) 
        {
            try
            {
                await _flightRepository.GetAsync(flight.FlightNumber);
                throw new FlightAlreadyExistsException();
            }
            catch(NoSuchFlightException)
            {
                flight.Status = 1;
                flight = await _flightRepository.Add(flight);
                _logger.LogInformation("Flight added from Service method");
                return flight;
            }
        }

        public async Task<List<Flight>> GetAllFlights()
        {
            var flights = await _flightRepository.GetAsync();
            return flights;
        }

        public async Task<List<Flight>> GetAllFlightsById(int flightOwnerId)
        {
            var flights = await _flightRepository.GetAsync();
            List<Flight> ownedFlights = new List<Flight>();

            foreach (var flight in flights)
            {
                if(flight.FlightOwnerId == flightOwnerId)
                {
                    ownedFlights.Add(flight);
                }
            }

            if(ownedFlights != null)
            {
                return ownedFlights;
            }

            throw new NoSuchFlightException();
        }

        public async Task<Flight> GetFlightbyId(string flightNo)
        {
            var flight = await _flightRepository.GetAsync(flightNo);
            if(flight != null)
            {
                return flight;
            }

            throw new NoSuchFlightException();
        }

        public async Task<Flight> RemoveFlight(string flightNumber)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if(flight != null)
            {
                flight.Status = 0;
                flight = await _flightRepository.Delete(flightNumber);
                _logger.LogInformation("Flight is removed and status changed to 0");
                return flight;
            }

            throw new NoSuchFlightException();
        }

        public async Task<Flight> UpdateFlight(string flightNumber, string airline)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if(flight != null)
            {
                flight.Airline = airline;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight Airline updated from service method");
                return flight;
            }

            throw new NoSuchFlightException();
        }

        public async Task<bool> UpdateFlightStatus(string flightNumber,int status)
        {
            var flights = await _flightRepository.GetAsync();
            var flight = flights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if(flight != null)
            {
                flight.Status = status;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight Totalseats updated from service method");
                return true;
            }

            throw new NoSuchFlightException();
        }

        public async Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if(flight != null)
            {
                flight.TotalSeats = totalSeats;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight Totalseats updated from service method");
                return flight;
            }

            throw new NoSuchFlightException();
        }
    }
}
