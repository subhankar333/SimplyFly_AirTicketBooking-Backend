using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleFlightOwnerService _scheduleFlightOwnerService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(IScheduleFlightOwnerService scheduleFlightOwnerService, ILogger<ScheduleController> logger)
        {
            _scheduleFlightOwnerService = scheduleFlightOwnerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<List<Schedule>>> GetAllSchedule()
        {
            try
            {
                var schedules = await _scheduleFlightOwnerService.GetAllSchedules();
                return schedules;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("FlightSchedule")]
        [HttpGet]
        public async Task<ActionResult<List<FlightScheduleDTO>>> GetFlightSchedule([FromQuery] string flightNumber)
        {
            try
            {
                var flightSchedule = await _scheduleFlightOwnerService.GetFlightSchedule(flightNumber);
                return flightSchedule;
            }
            catch (NoSuchScheduleException ex)

            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Schedule>> AddSchedule(Schedule schedule)
        {
            try
            {
                schedule = await _scheduleFlightOwnerService.AddSchedule(schedule);
                return schedule;
            }
            catch (FlightScheduleBusyException ex)

            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [Route("UpdateScheduledFlight")]
        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledFlight(ScheduleFlightDTO scheduleFlightDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService. UpdateScheduledFlight(scheduleFlightDTO.ScheduleId, scheduleFlightDTO.FlightNumber);
                return schedule;
            }
            catch (NoSuchScheduleException ex)

            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("UpdateScheduledRoute")]
        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledRoute(ScheduleRouteDTO scheduleRouteDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.
                UpdateScheduledRoute(scheduleRouteDTO.ScheduleId, scheduleRouteDTO.RouteId);
                return schedule;
            }
            catch (NoSuchScheduleException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("UpdateScheduledTime")]
        [HttpPut]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<Schedule>> UpdateScheduledTime(ScheduleTimeDTO scheduleTimeDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.
                UpdateScheduledTime(scheduleTimeDTO.ScheduleId, scheduleTimeDTO.DepartureTime,
                scheduleTimeDTO.ArrivalTime);
                return schedule;
            }
            catch (NoSuchScheduleException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Route("DeleteScheduleByFlight")]
        [HttpDelete]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<int>> DeleteScheduleByFlight(string flightNumber)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.RemoveSchedule(flightNumber);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("DeleteScheduleByDate")]
        [HttpDelete]
        [Authorize(Roles = "flightowner")]
        public async Task<ActionResult<int>> DeleteScheduleByDate(RemoveScheduleDateDTO scheduleDTO)
        {
            try
            {
                var schedule = await _scheduleFlightOwnerService.RemoveSchedule(scheduleDTO.DateOfSchedule, scheduleDTO.AirportId);
                return schedule;
            }
            catch (NoSuchScheduleException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

    }
}
