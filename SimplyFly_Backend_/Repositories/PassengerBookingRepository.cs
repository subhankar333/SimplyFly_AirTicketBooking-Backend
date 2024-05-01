using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PassengerBookingRepository : IRepository<int, PassengerBooking>, IPassengerBookingRepository
    {

        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<PassengerBookingRepository> _logger;

        public PassengerBookingRepository(SimplyFlyDbContext context, ILogger<PassengerBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PassengerBooking> Add(PassengerBooking item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"PassengerBooking added with PassengerBookingId {item.PassengerBookingId}");
            return item;
        }

        public async Task AddPassengerBookingAsync(PassengerBooking passengerBooking)
        {
            _context.Add(passengerBooking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckSeatsAvailbilityAsync(int scheduleId, List<string> seatNos)
        {
            // Get all seats for the given seatNos List
            var bookedSeats = await _context.PassengersBookings
                .Where(pb => pb.Booking.ScheduleId == scheduleId &&  seatNos.Contains(pb.SeatNumber))
                .Select(pb => pb.SeatNumber)
                .ToListAsync();

            
            foreach (var seatNo in seatNos)
            {
                if (bookedSeats.Contains(seatNo))
                {
                    // Seat is already booked, return false
                    throw new Exception($"Seat {seatNo} is already booked.");

                }
            }

            // All seats are available
            return true;
        }

        public async Task<PassengerBooking> Delete(int passengerBookingId)
        {
            var passengerBooking = await GetAsync(passengerBookingId);
            if (passengerBooking != null)
            {
                _context.Remove(passengerBooking);
                _context.SaveChanges();
                _logger.LogInformation($"PassengerBooking delete with id {passengerBookingId}");
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        public async Task<PassengerBooking> GetAsync(int passengerBookingId)
        {
            var passengerBookings = await GetAsync();
            var passengerBooking = passengerBookings.FirstOrDefault(pb => pb.PassengerBookingId == passengerBookingId);
            if (passengerBooking != null)
            {
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        public async Task<List<PassengerBooking>> GetAsync()
        {
           var passengerBookings = _context.PassengersBookings.Include(pb => pb.Booking).Include(pb => pb.Booking.Schedule)
                .Include(pb => pb.Passenger).Include(pb => pb.SeatDetail).Include(pb => pb.Booking.Schedule.Route)
                .Include(e => e.Booking.Schedule.Flight).Include(e => e.Booking.Schedule.Route.SourceAirport)
                .Include(pb => pb.Booking.Schedule.Route.DestinationAirport)
                .ToList();
            return passengerBookings;
        }

        public async Task<IEnumerable<PassengerBooking>> GetPassengerBookingAsync(int bookingId)
        {
            return await _context.PassengersBookings
                .Where(pb => pb.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task RemovePassengerBookingAsync(int passengerBookingId)
        {
            var passengerBooking = await _context.PassengersBookings.FindAsync(passengerBookingId);
            if (passengerBooking != null)
            {
                _context.PassengersBookings.Remove(passengerBooking);
                await _context.SaveChangesAsync();
            }

            throw new NoSuchPassengerBookingException();
        }

        public async Task<PassengerBooking> Update(PassengerBooking item)
        {
            var passengerBooking = await GetAsync(item.PassengerBookingId);
            if (passengerBooking != null)
            {
                _context.Entry<Models.PassengerBooking>(passengerBooking).State = EntityState.Modified;
                _context.SaveChanges();
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }
    }
}
