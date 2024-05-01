using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class SeatDetailService : ISeatDetailService
    {

        private readonly IRepository<string, SeatDetail> _seatDetailRepository;
        private readonly ILogger<SeatDetailService> _logger;

        public SeatDetailService(IRepository<string, SeatDetail> seatDetailRepository, ILogger<SeatDetailService> logger)
        {
            _seatDetailRepository = seatDetailRepository;
            _logger = logger;
        }
        public async Task<SeatDetail> AddSeatDetail(SeatDetail seatDetail)
        {
            return await _seatDetailRepository.Add(seatDetail); 
        }

        public async Task<List<SeatDetail>> GetAllSeatDetails()
        {
            return await _seatDetailRepository.GetAsync();
        }

        public async Task<SeatDetail> GetSeatDetailById(string seatNo)
        {
            return await _seatDetailRepository.GetAsync(seatNo);
        }

        public async Task<bool> RemoveSeatDetail(string seatNo)
        {
            var seatDetail = await _seatDetailRepository.GetAsync(seatNo);
            if(seatDetail != null)
            {
                await _seatDetailRepository.Delete(seatNo);
                return true;
            }

            return false;
        }
    }
}
