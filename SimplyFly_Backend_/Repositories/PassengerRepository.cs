using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PassengerRepository:IRepository<int,Passenger>
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<PassengerRepository> _logger;

        public PassengerRepository(SimplyFlyDbContext context, ILogger<PassengerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Passenger> Add(Passenger item)
        {
            try
            {
                _context.Add(item);
                _context.SaveChanges();
                _logger.LogInformation("Passenger added with PassengerId " + item.PassengerId);
                return item;
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"{ex.InnerException.Message}");
                throw ex;
            }
        }

        public async Task<Passenger> Delete(int passengerId)
        {
            var passenger = await GetAsync(passengerId);
            if(passenger != null)
            {
                _context.Remove(passenger);
                _context.SaveChanges();
                _logger.LogInformation($"Passenger deleted with PassengerId {passengerId}");
                return passenger;
            }

            throw new NoSuchPassengerException();
        }

        public async Task<Passenger> GetAsync(int passengerId)
        {
            var passengers = await GetAsync();
            var passenger = passengers.FirstOrDefault(p => p.PassengerId == passengerId);
            if(passenger != null)
            {
                return passenger;
            }

            throw new NoSuchPassengerException();
        }

        public async Task<List<Passenger>> GetAsync()
        {
            var passengers = _context.Passengers.ToList();
            return passengers;
        }

        public async Task<Passenger> Update(Passenger item)
        {
            var passenger = await GetAsync(item.PassengerId);
            if (passenger != null)
            {
                _context.Entry<Models.Passenger>(passenger).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Passenger updated with PassengerId {item.PassengerId}");
                return passenger;
            }

            throw new NoSuchPassengerException();
        }
    }
}
