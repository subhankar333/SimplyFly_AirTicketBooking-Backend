using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class PassengerService : IPassengerService
    {

        private readonly IRepository<int, Passenger> _passengerRepository;
        private readonly ILogger<PassengerService> _logger;

        public PassengerService(IRepository<int, Passenger> passengerRepository, ILogger<PassengerService> logger)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;
        }
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            try
            {
              return await _passengerRepository.Add(passenger);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.InnerException.Message);
                throw;
            }
        }

        public async Task<bool> RemovePassenger(int passengerId)
        {
            var passenger = await _passengerRepository.GetAsync(passengerId);
            if(passenger != null)
            {
                await _passengerRepository.Delete(passengerId);
                return true;
            }

            return false;
        }

        public async Task<List<Passenger>> GetAllPassengers()
        {
            return await _passengerRepository.GetAsync();
        }

        public async Task<Passenger> GetPassengerById(int passengerId)
        {
            return await _passengerRepository.GetAsync(passengerId);
        } 
       
    }
}
