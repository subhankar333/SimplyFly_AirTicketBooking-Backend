using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IPassengerService
    {
        Task<Passenger> AddPassenger(Passenger passenger);
        Task<bool> RemovePassenger(int passengerId);
        Task<List<Passenger>> GetAllPassengers();
        Task<Passenger> GetPassengerById(int passengerId);
    }
}
