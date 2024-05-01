using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<CustomerDashboardController> _logger;

        public CustomerDashboardController(IUserService userService, ICustomerService customerService, IBookingService bookingService, ILogger<CustomerDashboardController> logger)
        {
            _userService = userService;
            _customerService = customerService;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("{customerId}/Bookings")]
        public async Task<IActionResult> BookTickets(int customerId, [FromBody] BookingRequestDTO bookingRequestDTO)
        {
            try
            {
                bookingRequestDTO.CustomerId = customerId;
                var bookingId = await _bookingService.CreateBookingAsync(bookingRequestDTO);
                return Ok(bookingId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{customerId}/Bookings/{bookingId}")]
        public async Task<IActionResult> GetBooking(int customerId,int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if(booking == null || booking.CustomerId != customerId)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpGet("GetCustomerByUsername")]
        public async Task<ActionResult<Customer>> GetCustomerByUserName(string userName)
        {
            try
            {
                var customer = await _customerService.GetCustomerByUsername(userName);
                return Ok(customer);
            }
            catch(NoSuchCustomerException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(customerId);
                return Ok(customer);
            }
            catch (NoSuchCustomerException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("{customerId}/bookings/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);
            if (success != null)
            {
                return NoContent();
            }
            return NotFound();
        }


        [HttpGet("{customerId}/GetBookings")]
        public async Task<IActionResult> GetBookingHistory(int customerId)
        {
            var bookingHistory = await _bookingService.GetUserBookingsAsync(customerId);
            return Ok(bookingHistory);
        }

        [Route("GetBookingByCustomerId")]
        [HttpGet]
        public async Task<ActionResult<List<PassengerBooking>>> GetBookingByCustomerId(int customerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByCustomerId(customerId);
                return Ok(bookings);
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogError(nsce.Message);
                return NotFound();
            }

        }

        [HttpPut("{customerId}/bookings/{bookingId}/refund")]
        public async Task<IActionResult> RequestRefund(int bookingId, int cancelBookingId)
        {
            var success = await _bookingService.RequestRefundAsync(bookingId,cancelBookingId);
            if (success)
            {
                return Ok("Refund requested successfully.");
            }
            return NotFound();
        }


        [Route("UpdateUser")]
        [Authorize(Roles = "customer")]
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomer(UpdateCustomerDTO customerDTO)
        {
            try
            {
                var customer = await _customerService.UpdateCustomer(customerDTO);
                return customer;
            }
            catch (NoSuchCustomerException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("CancelBookingByPassenger")]
        [Authorize(Roles = "customer")]
        [HttpDelete]
        public async Task<ActionResult<PassengerBooking>> CancelBookingByPassenger(int passengerId)
        {
            try
            {
                var passengerBooking = await _bookingService.CancelBookingByPassenger(passengerId);
                return passengerBooking;
            }
            catch (NoSuchBookingsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("GetAllCancelledBookings")]
        [HttpGet]
        public async Task<ActionResult<List<CancelledBooking>>> GetAllCancelledBookings()
        {
            try
            {
                var cancelledBookings = await _bookingService.GetAllCancelledBookings();
                return cancelledBookings;
            }
            catch (NoSuchBookingsException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
