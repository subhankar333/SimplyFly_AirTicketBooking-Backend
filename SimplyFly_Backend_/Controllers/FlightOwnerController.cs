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
    public class FlightOwnerController : ControllerBase
    {
       private readonly IFlightOwnerService _flightOwnerService;
       private readonly ILogger<FlightOwnerController> _logger;

        public FlightOwnerController(IFlightOwnerService flightOwnerService, ILogger<FlightOwnerController> logger)
        {
            _flightOwnerService = flightOwnerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<FlightOwner>> GetFlightOwnerByUsername(string username)
        {
            try
            {
                var flightOwner = await _flightOwnerService.GetFlightOwnerByUsername(username);
                return Ok(flightOwner);
            }
            catch(NoSuchFlightOwnerException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);  
            }
        }

        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<FlightOwner>> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner)
        {
            try
            {
                var _flightOwner = await _flightOwnerService.UpdateFlightOwner(flightOwner);
                return Ok(_flightOwner);
            }
            catch(NoSuchFlightOwnerException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateRefundStatus")]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<FlightOwner>> UpdateRefundStatus(int cancelBookingId,string status)
        {
            try
            {
                var _cancelledBooking = await _flightOwnerService.UpdateRefundStatus(cancelBookingId, status);
                return Ok(_cancelledBooking);
            }
            catch (NoSuchBookingsException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
