using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Route:IEquatable<Route>
    {
        [Key]
        public int RouteId { get; set; }
        public int SourceAirportId { get; set; }

        [ForeignKey("SourceAirportId")]
        public Airport? SourceAirport { get; set; }


        public int DestinationAirportId { get; set; }

        [ForeignKey("DestinationAirportId")]
        public Airport? DestinationAirport { get; set; }


        public int Distance { get; set; }
        public int? Status { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }

        public Route()
        {
            RouteId = 0;
        }

        public Route(int routeId, int sourceAirportId, Airport? sourceAirport, int destinationAirportId, Airport? destinationAirport)
        {
            RouteId = routeId;
            SourceAirportId = sourceAirportId;
            SourceAirport = sourceAirport;
            DestinationAirportId = destinationAirportId;
            DestinationAirport = destinationAirport;
        }

        public bool Equals(Route? other)
        {
            var route = other ?? new Route();
            return this.RouteId.Equals(route.RouteId);
        }
    }
}
