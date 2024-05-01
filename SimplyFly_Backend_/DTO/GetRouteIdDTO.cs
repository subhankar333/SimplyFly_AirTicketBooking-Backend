using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class GetRouteIdDTO
    {
        public int SourceAirportId { get; set; }
        public int DestinationAirportId { get; set; }
    }
}
