using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface ISeatDetailRepository
    {
        Task<IEnumerable<SeatDetail>> GetSeatDetailsAsync(List<string> seatNos);
        Task UpdateSeatDetailsAsync(IEnumerable<SeatDetail> seatDetails);

    }
}
