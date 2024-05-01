using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IFlightOwnerService
    {
        public Task<FlightOwner> AddFlightOwner(FlightOwner flightOwner);
        public Task<bool> RemoveFlightOwner(int flightOwnerId);
        public Task<FlightOwner> GetFlightOwnerById(int flightOwnerId);
        public Task<List<FlightOwner>> GetAllFlightOwners();
        public Task<FlightOwner> GetFlightOwnerByUsername(string username);
        public Task<FlightOwner> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner);
        public Task<CancelledBooking> UpdateRefundStatus(int cancelBookingId, string status);

    }
}
