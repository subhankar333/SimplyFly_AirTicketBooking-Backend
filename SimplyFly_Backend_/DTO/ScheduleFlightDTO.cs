using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class ScheduleFlightDTO
    {
        public int ScheduleId { get; set; }
        public string FlightNumber { get; set; }
    }
}
