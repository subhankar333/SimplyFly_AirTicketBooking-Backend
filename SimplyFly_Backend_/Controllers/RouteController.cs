using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
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
    public class RouteController : ControllerBase
    {
        private readonly IRouteFlightOwnerService _routeFlightOwnerService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteFlightOwnerService routeFlightOwnerService, ILogger<RouteController> logger)
        {
            _routeFlightOwnerService = routeFlightOwnerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "flightowner, admin")]
        public async Task<ActionResult<List<Models.Route>>> GetAllRoute()
        {
            try
            {
                var routes = await _routeFlightOwnerService.GetAllRoutes();
                return routes;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }

        [Route("GetRouteById")]
        [HttpGet]
        public async Task<ActionResult<int>> GetRouteById([FromQuery] GetRouteIdDTO routeIdDTO)
        {
            try
            {
                int routeId = await _routeFlightOwnerService.GetRouteIdByAirport(routeIdDTO.SourceAirportId, routeIdDTO.DestinationAirportId);
                return Ok(routeId);
            }
            catch (NoSuchRouteException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("GetAirportName")]
        [HttpGet]
        public async Task<ActionResult<Airport>> GetAirportName([FromQuery] int routeId)
        {
            try
            {
                Airport airport = await _routeFlightOwnerService.GetAirportName(routeId);
                return Ok(airport);
            }
            catch (NoSuchAirportException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [Route("GetAirports")]
        [HttpGet]
        public async Task<ActionResult<List<Airport>>> GetAirports()
        {
            try
            {
                var airports = await _routeFlightOwnerService.GetAllAirports();
                return airports;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [Route("AddAirport")]
        [Authorize(Roles = "flightowner")]
        [HttpPost]
        public async Task<ActionResult<Airport>> AddAirport(Airport airport)
        {
            try
            {
                var _airport = _routeFlightOwnerService.AddAirport(airport);
                return Ok(_airport);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("AddRoute")]
        [HttpPost]
        [Authorize(Roles = "flightowner, admin")]
        public async Task<ActionResult<Models.Route>> AddRoute(Models.Route route)
        {
            try
            {
                route = await _routeFlightOwnerService.AddRoute(route);
                return route;
            }
            catch (RouteAlreadyExistsException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "flightowner,admin")]
        public async Task<ActionResult<Models.Route>> RemoveRoute(RemoveRouteDTO removeRoute)
        {
            try
            {
                var route = _routeFlightOwnerService.RemoveRoute(removeRoute.sourceAirportId, removeRoute.destinationAirportId);
                return Ok(route);
            }
            catch(NoSuchRouteException ex) 
            { 
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
