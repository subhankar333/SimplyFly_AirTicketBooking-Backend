using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BookingRepository : IRepository<int, Booking>, IBookingRepository
    {
        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(SimplyFlyDbContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Booking> Add(Booking item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"Booking added with id {item.BookingId}");
            return item;
        }

        public async Task<Booking> Delete(int bookingId)
        {
            var booking = await GetAsync(bookingId);
            if (booking != null)
            {
                _context.Remove(booking);
                _context.SaveChanges();
                _logger.LogInformation($"Booking deleted with id {bookingId}");
                return booking;
            }
            throw new NoSuchBookingsException();
        }


        public async Task<List<Booking>> GetAsync()
        {
            var bookings = _context.Bookings.Include(e => e.Schedule).Include(e => e.Payment)
                .Include(e => e.Schedule.Route).Include(e => e.Schedule.Flight)
                .Include(e => e.Schedule.Route.SourceAirport).Include(e => e.Schedule.Route.DestinationAirport)
                .ToList();

            return bookings;
        }

        public async Task<Booking> GetAsync(int bookingId)
        {
            var bookings = await GetAsync();
            var booking = bookings.FirstOrDefault(b => b.BookingId ==  bookingId);

            if(booking != null)
            {
               return booking;
            }

            throw new NoSuchBookingsException();

        }


        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _context.Bookings.Where(b => b.CustomerId ==  customerId).ToListAsync();
        }

        public async Task<Booking> Update(Booking item)
        {
            var bookings = await GetAsync(item.BookingId);
            if (bookings != null)
            {
                _context.Entry<Booking>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Booking updated with id {item.BookingId}");
                return bookings;
            }
            throw new NoSuchBookingsException();
        }
    }
}
