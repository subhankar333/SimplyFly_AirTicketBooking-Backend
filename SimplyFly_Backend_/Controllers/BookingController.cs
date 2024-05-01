using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [Authorize(Roles = "flightowner")]
        [Route("GetBookingByFlight")]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookingByFlight(string flightNumber)
        {
            try
            {
                var bookings = _bookingService.GetBookingByFlight(flightNumber);
                return Ok(bookings);
            }
            catch(NoSuchBookingsException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [Route("GetBookedSeats")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetBookedSeats(int scheduleId)
        {
            try
            {
                var bookedSeats = await _bookingService.GetBookedSeatBySchedule(scheduleId);
                return bookedSeats;
            }
            catch (NoSuchBookingsException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }
    }
}
