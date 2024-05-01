using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class ScheduleTimeDTO
    {
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
