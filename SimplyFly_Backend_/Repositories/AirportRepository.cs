using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AirportRepository : IRepository<int, Airport>
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<AirportRepository> _logger;

        public AirportRepository(SimplyFlyDbContext context, ILogger<AirportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Airport> Add(Airport item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Airport added with AirportId " + item.AirportId);
            return item;
        }

        public Task<Airport> Delete(int airportId)
        {
            var airport = GetAsync(airportId);
            if (airport != null)
            {
                _context.Remove(airport);
                _context.SaveChanges();
                _logger.LogInformation($"Airport removed with id {airportId}");
                return airport;
            }
            throw new NoSuchAirportException();
        }

        public async Task<Airport> GetAsync(int airportId)
        {
            var airports = await GetAsync();
            var airport = airports.FirstOrDefault(e => e.AirportId == airportId);
            if (airport != null)
            {
                return airport;
            }
            throw new NoSuchAirportException();
        }

        public async Task<List<Airport>> GetAsync()
        {
            var airports = _context.Airports.ToList();
            return airports;
        }

        public async Task<Airport> Update(Airport item)
        {
            var airport = await GetAsync(item.AirportId);
            if (airport != null)
            {
                _context.Entry<Airport>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Airport updated with id {item.AirportId}");
                return airport;
            }
            throw new NoSuchAirportException();
        }
    }
}
