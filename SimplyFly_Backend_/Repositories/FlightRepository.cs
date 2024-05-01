using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FlightRepository : IRepository<string, Flight>
    {
        private readonly SimplyFlyDbContext _context;
        ILogger<FlightRepository> _logger;

        public FlightRepository(SimplyFlyDbContext context, ILogger<FlightRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Flight> Add(Flight item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Flight added with FlightNo" + item.FlightNumber);
            return item;
        }

        public async Task<Flight> Delete(string flightNumber)
        {
            var flight = await GetAsync(flightNumber);
            if (flight != null)
            {
                flight.Status = 0;
                _context.SaveChanges();
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> GetAsync(string flightNumber)
        {
            var flights = _context.Flights.ToList();
            var flight = flights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (flight != null)
            {
                return flight;
            }

            throw new NoSuchFlightException();
        }

        public async Task<List<Flight>> GetAsync()
        {
            var flights = _context.Flights.ToList();
            return flights;
        }

        public async Task<Flight> Update(Flight item)
        {
            var flight = await GetAsync(item.FlightNumber);
            if (flight != null)
            {
                _context.Entry<Flight>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Flight updated with FlightNo" + item.FlightNumber);
                return flight;
            }

            throw new NoSuchFlightException();
        }
    }
}
