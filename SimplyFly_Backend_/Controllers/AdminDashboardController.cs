using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Services;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFlightOwnerService _flightOwnerService;
        private readonly IFlightFlightOwnerService _flightFlightOwnerService;
        private readonly IBookingService _bookingService;
        private readonly IRouteFlightOwnerService _routeFlightOwnerService;
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminDashboardController> _logger;

        public AdminDashboardController(ICustomerService customerService, IFlightOwnerService flightOwnerService,IFlightFlightOwnerService flightFlightOwnerService, IBookingService bookingService, IRouteFlightOwnerService routeFlightOwnerService,IAdminService adminService, ILogger<AdminDashboardController> logger)
        {
            _customerService = customerService;
            _flightOwnerService = flightOwnerService;
            _flightFlightOwnerService = flightFlightOwnerService;
            _bookingService = bookingService;
            _routeFlightOwnerService = routeFlightOwnerService;
            _adminService = adminService;
            _logger = logger;
        }


        [HttpGet("Bookings/Allbookings")]
        [Authorize(Roles = "admin, flightowner")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }


        [HttpGet("Users/AllCustomers")]
        public async Task<IActionResult> GetAsync()
        {
            var customers = await _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("Users/AllFlightOwners")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetFlightOwnersAsync()
        {
            var flightOwners = await _flightOwnerService.GetAllFlightOwners();
            return Ok(flightOwners);
        }


        [Route("GetAdminByUsername")]
        [HttpGet]
        public async Task<ActionResult<Admin>> GetAdminByUsername(string username)
        {
            try
            {
                var admin = await _adminService.GetAdminByUsername(username);
                return Ok(admin);
            }
            catch (NoSuchAdminException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("customers/{customerId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            var result = await _customerService.RemoveCustomer(customerId);

            if (result)
            {
                return Ok("Customer deleted successfully.");
            }

            return NotFound("Customer not found.");
        }

        [HttpDelete("flightowners/{flightOwnerId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteFlightOwner(int flightOwnerId)
        {
            var result = await _flightOwnerService.RemoveFlightOwner(flightOwnerId);

            if (result)
            {
                return Ok("FlightOwner deleted successfully.");
            }

            return NotFound("FlightOwner not found.");

        }



        [HttpDelete("bookings/{bookingId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);

            if (success != null)
            {
                return NoContent();
            }
            return NotFound();
        }


        [HttpDelete("flightroutes/{flightRouteId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteFlightRoute(int flightRouteId)
        {
            var result = await _routeFlightOwnerService.RemoveRouteById(flightRouteId);

            if (result)
            {
                return Ok("Flight route deleted successfully.");
            }
            return NotFound("Flight route not found.");
        }


        [Route("DeleteUserByUsername")]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> DeleteUserByUsername(string username)
        {
            try
            {
                var user = await _adminService.DeleteUser(username);
                return Ok(user);
            }
            catch (NoSuchUserException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Admin>> UpdateAdmin(UpdateAdminDTO adminDTO)
        {
            try
            {
                var admin = await _adminService.UpdateAdmin(adminDTO);
                return admin;
            }
            catch (NoSuchAdminException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
