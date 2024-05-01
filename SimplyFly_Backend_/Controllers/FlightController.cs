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
    public class FlightController : ControllerBase
    {
        private readonly IFlightFlightOwnerService _flightFlightOwnerService;
        private readonly IFlightCustomerService _flightCustomerService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightFlightOwnerService flightFlightOwnerService, IFlightCustomerService flightCustomerService, ILogger<FlightController> logger)
        {
            _flightFlightOwnerService = flightFlightOwnerService;
            _flightCustomerService = flightCustomerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<List<Flight>>> GetAllFlight()
        {
            try
            {
                var flights = await _flightFlightOwnerService.GetAllFlights();
                return Ok(flights);
            }
            catch(Exception)
            {
                _logger.LogInformation("You are not authorised user");
                return NotFound("You are not authorised user");
            }
        }


        [HttpGet("GetAllFlights/flightOwnerId")]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<List<Flight>>> GetAllFlightsById(int flightOwnerId)
        {
            try
            {
                var flights = await _flightFlightOwnerService.GetAllFlightsById(flightOwnerId);
                return Ok(flights);
            }
            catch (Exception)
            {
                _logger.LogInformation("You are not authorised user");
                return NotFound("You are not authorised user");
            }
        }


        [Route("SearchFlight")]
        [HttpGet]
        public async Task<ActionResult<List<SearchedFlightResultDTO>>> GetAllFlights([FromQuery] SearchFlightDTO searchFlightDTO)
        {
            try
            {
                var flights = await _flightCustomerService.SearchFlights(searchFlightDTO);
                return flights;
            }
            catch (NoFlightAvailableException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [Route("AddFlight")]
        [HttpPost]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Flight>> AddFlight(Flight flight)
        {
            try
            {
                flight = await _flightFlightOwnerService.AddFlight(flight);
                return flight;
            }
            catch (FlightAlreadyExistsException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }

        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Flight>> UpdateFlightAirline(FlightAirlineDTO flightDTO)
        {

            try
            {
                var flight = await _flightFlightOwnerService.UpdateFlight(flightDTO.FlightNumber, flightDTO.Airline);
                return flight;
            }
            catch (NoSuchFlightException ex)

            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }


        [Route("UpdateTotalSeats")]
        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Flight>> UpdateTotalSeats(FlightSeatsDTO flightSeats)
        {
            try
            {
                var flight = await _flightFlightOwnerService.UpdateTotalSeats(flightSeats.FlightNumber, flightSeats.TotalSeats);
                return flight;
            }
            catch (NoSuchFlightException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }


        [Route("UpdateFlightStatus")]
        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<bool>> UpdateFlightStatus(string flightNumber,int status)
        {
            try
            {
                bool result = await _flightFlightOwnerService.UpdateFlightStatus(flightNumber, status);
                return result;
            }
            catch (NoSuchFlightException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }


        [HttpDelete]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Flight>> RemoveFlight(string flightNumber)
        {
            try
            {
                var flight = await _flightFlightOwnerService.RemoveFlight(flightNumber);
                return flight;
            }
            catch (NoSuchFlightException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }


        }


        [Route("AvailableSeats")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAvailableSeats(string flightNumber)
        {
            try
            {
                var availableSeats = await _flightCustomerService.GetAvailableSeatsByFlightNo(flightNumber);
                return Ok(availableSeats);
            }
            catch (NoSuchFlightException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        //[Route("GetTotalPrice")]
        [HttpGet("GetTotalPrice")]
        public async Task<ActionResult<double>> GetTotalPrice(string flightNo, [FromQuery] List<string> seats,[FromQuery] List<int> passengerIds)
        {
            try
            {
                double totalprice = await _flightCustomerService.CalculatePrice(flightNo, seats, passengerIds);
                return Ok(totalprice);
            }
            catch (InvalidSelectionWithFlight ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
