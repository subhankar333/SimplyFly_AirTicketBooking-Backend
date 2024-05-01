using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FlightOwnerRepository : IRepository<int, FlightOwner>
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<FlightOwnerRepository> _logger;

        public FlightOwnerRepository(SimplyFlyDbContext context, ILogger<FlightOwnerRepository> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<FlightOwner> Add(FlightOwner item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("FlightOwner added " + item.FlightOwnerId);
            return item;
        }

        public async Task<FlightOwner> Delete(int flightOwnerId)
        {
            var flightOwner = await GetAsync(flightOwnerId);
            if (flightOwner != null)
            {
                _context?.Remove(flightOwner);
                _context.SaveChanges();
                _logger.LogInformation("FlightOwner deleted with id" + flightOwnerId);
                return flightOwner;
            }

            throw new NoSuchFlightOwnerException();
        }

        public async Task<FlightOwner> GetAsync(int flightOwnerId)
        {
            var flightOwners = await GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.FlightOwnerId == flightOwnerId);
            if (flightOwner != null)
            {
                return flightOwner;
            }

            throw new NoSuchFlightOwnerException();
        }

        public async Task<List<FlightOwner>> GetAsync()
        {
            var flightOwners = _context.FlightOwners.ToList();
            return flightOwners;
        }

        public async Task<FlightOwner> Update(FlightOwner item)
        {
            var flightOwner = await GetAsync(item.FlightOwnerId);

            _context.Entry<FlightOwner>(item).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogInformation("FlightOwner updated with id" + item.FlightOwnerId);
            return flightOwner;

        }
    }
}
