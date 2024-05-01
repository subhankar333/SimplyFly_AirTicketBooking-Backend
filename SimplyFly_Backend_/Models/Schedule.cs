using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Schedule:IEquatable<Schedule>
    {
        [Key]
        public int ScheduleId { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route? Route { get; set; }
        public String FlightNumber { get; set; }

        [ForeignKey("FlightNumber")]
        public Flight? Flight { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }


        public Schedule()
        {
            ScheduleId = 0;
        }

        public Schedule(int scheduleId, int routeId, Route? route, string flightNumber, DateTime departure, DateTime arrival)
        {
            ScheduleId = scheduleId;
            RouteId = routeId;
            Route = route;
            Departure = departure;
            Arrival = arrival;
            FlightNumber = flightNumber;
        }
        public Schedule(int routeId, Route? route, string flightNumber, DateTime departure, DateTime arrival)
        {

            RouteId = routeId;
            Route = route;
            Departure = departure;
            Arrival = arrival;
            FlightNumber = flightNumber;
        }

        public bool Equals(Schedule? other)
        {
            var schedule = other ?? new Schedule();
            return this.ScheduleId.Equals(schedule.ScheduleId);
        }
    }
}
