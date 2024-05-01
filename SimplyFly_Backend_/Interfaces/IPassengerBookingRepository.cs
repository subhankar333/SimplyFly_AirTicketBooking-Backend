using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IPassengerBookingRepository
    {
        Task AddPassengerBookingAsync(PassengerBooking passengerBooking);
        Task<IEnumerable<PassengerBooking>> GetPassengerBookingAsync(int bookingId);
        Task RemovePassengerBookingAsync(int passengerBookingId);
        Task<bool> CheckSeatsAvailbilityAsync(int scheduleId, List<string> seatNos);
    }
}
