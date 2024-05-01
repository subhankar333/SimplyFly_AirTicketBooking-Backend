using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface ISeatDetailService
    {
        Task<SeatDetail> AddSeatDetail(SeatDetail seatDetail);
        Task<bool> RemoveSeatDetail(string seatDetailId);
        Task<List<SeatDetail>> GetAllSeatDetails();
        Task<SeatDetail> GetSeatDetailById(string seatDetailId);
    }
}
