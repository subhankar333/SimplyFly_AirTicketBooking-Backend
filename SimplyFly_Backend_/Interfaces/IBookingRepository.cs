using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
    }
}
