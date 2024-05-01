using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SeatDetailRepository : IRepository<string, SeatDetail>, ISeatDetailRepository
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<SeatDetailRepository> _logger;

        public SeatDetailRepository(SimplyFlyDbContext context, ILogger<SeatDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<SeatDetail> Add(SeatDetail item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"SeatDetail added with seatNo {item.SeatNo}");
            return item;
        }

        public async Task<SeatDetail> Delete(string seatNo)
        {
            var seatDetail = await GetAsync(seatNo);
            if(seatDetail != null)
            {
                _context.Remove(seatDetail);
                _context.SaveChanges();
                _logger.LogInformation("Seat detail deleted with seatDetailId " + seatNo);
                return seatDetail;
            }

            throw new NoSuchSeatException();
        }

        public async Task<SeatDetail> GetAsync(string seatNo)
        {
            var seatDetails = await GetAsync();
            var seatDetail = seatDetails.FirstOrDefault(sd => sd.SeatNo == seatNo);

            if(seatDetail != null)
            {
                return seatDetail;
            }

            throw new NoSuchSeatException();
        }

        public async Task<List<SeatDetail>> GetAsync()
        {
            var seatDetails = _context.SeatDetails.ToList();
            return seatDetails;
        }

        public async Task<IEnumerable<SeatDetail>> GetSeatDetailsAsync(List<string> seatNos)
        {
            // includes only the seats which are present in seatNos
            return await Task.FromResult(_context.SeatDetails.Where(sd => seatNos.Contains(sd.SeatNo)));
        }

        public async Task<SeatDetail> Update(SeatDetail item)
        {
            var seatDetail = await GetAsync(item.SeatNo);
            if (seatDetail != null)
            {
                _context.Entry(seatDetail).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Seat detail updated");
                return seatDetail;
            }
            throw new NoSuchSeatException();
        }

        public async Task UpdateSeatDetailsAsync(IEnumerable<SeatDetail> seatDetails)
        {
            //UpdateRange marks entities as modeified and later SaveChangesAsync updates the entity acc to the change done
            _context.SeatDetails.UpdateRange(seatDetails);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seat detail updated");
        }
    }
}
