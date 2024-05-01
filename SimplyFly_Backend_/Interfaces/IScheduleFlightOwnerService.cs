using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IScheduleFlightOwnerService
    {
        public Task<Schedule> AddSchedule(Schedule schedule);
        public Task<Schedule> RemoveSchedule(Schedule schedule);
        public Task<int> RemoveSchedule(string flightNo);
        public Task<int> RemoveSchedule(DateTime departureDate,int airportId);
        public Task<List<Schedule>> GetAllSchedules();
        public Task<List<FlightScheduleDTO>> GetFlightSchedule(string flightNumber);
        public Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber);
        public Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId);
        public Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival);

    }
}
