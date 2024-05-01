using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<List<Booking>> GetBookingBySchedule(int scheduleId);
        Task<bool> CreateBookingAsync(BookingRequestDTO bookingRequest);
        Task<Booking> CancelBookingAsync(int bookingId);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
        Task<List<CancelledBooking>> GetAllCancelledBookings();
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<bool> RequestRefundAsync(int bookingId, int cancelBookingId);
        Task<List<Booking>> GetBookingByFlight(string flightNumber);
        Task<List<string>> GetBookedSeatBySchedule(int scheduleID);
        Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId);
        Task<PassengerBooking> CancelBookingByPassenger(int passengerId);
    }
}
