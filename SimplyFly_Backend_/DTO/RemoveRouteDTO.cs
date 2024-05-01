using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class RemoveRouteDTO
    {
        public int sourceAirportId { get; set; }
        public int destinationAirportId { get; set; }
    }
}
