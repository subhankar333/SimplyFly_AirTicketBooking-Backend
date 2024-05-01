using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class FlightSeatsDTO
    {
        public string FlightNumber { get; set; }
        public int TotalSeats { get; set; }
    }
}
