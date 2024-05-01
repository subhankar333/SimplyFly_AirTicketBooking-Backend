using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Airport:IEquatable<Airport>
    {
        [Key]
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }


        public Airport()
        {
            AirportId = 0;
        }

        public Airport(int airportId, string name, string city, string state, string country)
        {
            AirportId = airportId;
            Name = name;
            City = city;
            State = state;
            Country = country;
        }

        public bool Equals(Airport? other)
        {
            var airport = other ?? new Airport();
            return this.AirportId.Equals(airport.AirportId);
        }
    }
}
