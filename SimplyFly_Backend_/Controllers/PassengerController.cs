using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(IPassengerService passengerService, ILogger<PassengerController> logger)
        {
            _passengerService = passengerService;
            _logger = logger;
        }

        [HttpGet]
        public Task<List<Passenger>> GetAllPassengers()
        {
            var passengers = _passengerService.GetAllPassengers();
            return passengers;
        }

        [HttpGet("ById")]
        public Task<Passenger> GetPassenger(int passengerId)
        {
            var passenger = _passengerService.GetPassengerById(passengerId);
            return passenger;
        }

        [HttpPost]
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            try
            {
                passenger = await _passengerService.AddPassenger(passenger);
                return passenger;
            }
            catch (Exception ex)
            {
                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine($"Inner Exception: {innerException.Message}");
                    innerException = innerException.InnerException;
                }
                throw;
            }
        }
    }
}
