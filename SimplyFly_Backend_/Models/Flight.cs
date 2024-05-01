using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Flight:IEquatable<Flight>
    {
        [Key]
        public string FlightNumber { get; set; }
        public string Airline { get; set; } = string.Empty;
        public int TotalSeats { get; set; }

        [ForeignKey("FlightOwnerId")]
        public int FlightOwnerId { get; set; }

        public FlightOwner? FlightOwner { get; set; }

        public double BasePrice { get; set; }
        public int Status { get; set; }

        public Flight()
        {
            FlightNumber = string.Empty;

        }

        public Flight(string flightNumber, string airline, int totalSeats, int flightOwnerId, double basePrice)
        {
            FlightNumber = flightNumber;
            Airline = airline;
            TotalSeats = totalSeats;
            FlightOwnerId = flightOwnerId;
            BasePrice = basePrice;
        }

        public bool Equals(Flight? other)
        {
            var flight = other ?? new Flight();
            return this.FlightNumber.Equals(flight.FlightNumber);
        }
    }
}
